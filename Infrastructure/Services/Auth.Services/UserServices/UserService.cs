namespace Infrastructure
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<AddRoleToUserDto>> AddRoleToUserAsync(AddRoleToUserDto model)
        {
            var user = await _dbContext.Users.FindAsync(model.UserId);
            if (user == null) return new Response<AddRoleToUserDto>(HttpStatusCode.NotFound, "Пользователь не найден !");

            var role = await _dbContext.Roles.FindAsync(model.RoleId);
            if (role == null) return new Response<AddRoleToUserDto>(HttpStatusCode.NotFound, "Права доступа не найден !");

            var addRoleToUser = new UserRole
            {
                UserId = model.UserId,
                RoleId =model.RoleId,
                CreateDate=DateTimeOffset.UtcNow
            };

            await _dbContext.UserRoles.AddAsync(addRoleToUser);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0) return new Response<AddRoleToUserDto>(HttpStatusCode.InternalServerError, "Права доступа для пользователя не добавлена");
            return new Response<AddRoleToUserDto>(HttpStatusCode.OK, "Права доступа для пользователя успешно добавлена");
        }

        public async Task<PagedResponse<List<GetAllUsersDto>>> GetAllUsersAsync(UserFilter filter)
        {
            var validFilter = new UserFilter(filter.PageNumber, filter.PageSize);

            var users = _dbContext.Users.AsQueryable();

            if (filter.FirstNameOrLastName != null)
            {
                users = users.Where(x => x.FirstName.ToLower().Trim().Contains(filter.FirstNameOrLastName.ToLower().Trim())||
                x.LastName.ToLower().Trim().Contains(filter.FirstNameOrLastName.ToLower().Trim()));
            }

            var usersDto = await users.Select(user => new GetAllUsersDto
            {

                UserId = user.Id,
                Email =user.Email,
                IsBlocked =user.IsBlocked,
                FullName = string.Concat(user.LastName+" "+user.FirstName),
                Phone =user.Phone,
                UserName =user.UserName,
                UserType = user.UserType,
                Roles = user.Roles.Select(role => new GetAllRolesDto
                {
                    RoleId = role.Role.Id,
                    RoleName = role.Role.RoleName,
                    IsActive = role.Role.IsActive
                }).ToList()
                })
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            if (usersDto.Count == 0) return new PagedResponse<List<GetAllUsersDto>>(HttpStatusCode.NoContent, "No content ", usersDto.Count, validFilter.PageNumber, validFilter.PageSize);
            return new PagedResponse<List<GetAllUsersDto>>(usersDto, HttpStatusCode.OK, "All users ", usersDto.Count, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<Response<RemoveRoleFromUserDto>> RemoveRoleFromUserAsync(RemoveRoleFromUserDto model)
        {
            var user = await _dbContext.Users.FindAsync(model.UserId);
            if (user == null) return new Response<RemoveRoleFromUserDto>(HttpStatusCode.NotFound, "Пользователь не найден !");

            var role = await _dbContext.Roles.FindAsync(model.RoleId);
            if (role == null) return new Response<RemoveRoleFromUserDto>(HttpStatusCode.NotFound, "Права доступа не найден !");

            var userRole = await _dbContext.UserRoles.FindAsync(user.Id,role.Id);
            if (userRole == null) return new Response<RemoveRoleFromUserDto>(HttpStatusCode.BadRequest, "Права пользователя не найдена !");

            _dbContext.UserRoles.Remove(userRole);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0) return new Response<RemoveRoleFromUserDto>(HttpStatusCode.InternalServerError, "Права доступа от пользователя не удалена");
            return new Response<RemoveRoleFromUserDto>(HttpStatusCode.OK, "Права доступа от пользователя успешно удалена");
        }

        public async Task<Response<UpdateCurrentUserDto>> UpdateCurrentUserInfoAsync(UpdateCurrentUserDto model)
        {
            try
            {

                var currentUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

                if (currentUser == null) return new Response<UpdateCurrentUserDto>(HttpStatusCode.NotFound, "Текущий пользователь не найден !");


                currentUser.FirstName = currentUser.FirstName;
                currentUser.LastName = currentUser.LastName;
                currentUser.UserName = currentUser.UserName;
                currentUser.Email = currentUser.Email;
                currentUser.Phone = currentUser.Phone;

                var result = await _dbContext.SaveChangesAsync();

                if(result==0) return new Response<UpdateCurrentUserDto>(HttpStatusCode.InternalServerError, "Ваши данные не изменены !");
                return new Response<UpdateCurrentUserDto>(HttpStatusCode.OK, "Ваши данные успешно изменены !");
            }
            catch (Exception ex)
            {
                return new Response<UpdateCurrentUserDto>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public async Task<Response<GetCurrentUserDto>> GetCurrentUserInfoAsync(int userId)
        {
            try
            {
                var currentUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (currentUser == null) return new Response<GetCurrentUserDto>(HttpStatusCode.NotFound, "Текущий пользователь не найден !");

                var model = new GetCurrentUserDto
                {
                    UserId = currentUser.Id,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    UserName = currentUser.UserName,
                    Email = currentUser.Email,
                    Phone = currentUser.Phone
                };

                return new Response<GetCurrentUserDto>(HttpStatusCode.OK, "Данные текущего пользователя !", model);
            }
            catch (Exception ex)
            {
                return new Response<GetCurrentUserDto>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<Response<string>> BlockedUserAccountAsync(int userId)
        {
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (currentUser == null) return new Response<string>(HttpStatusCode.NotFound, "Пользователь не найден !");

            currentUser.IsBlocked = true;

            var result = await _dbContext.SaveChangesAsync();

            if (result == 0) return new Response<string>(HttpStatusCode.InternalServerError, "Аккаунт пользователя не заблокирован !");
            return new Response<string>(HttpStatusCode.OK, "Аккаунт пользователя успешно заблокирован !");
        }

        public async Task<Response<string>> UnBlockedUserAccountAsync(int userId)
        {
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (currentUser == null) return new Response<string>(HttpStatusCode.NotFound, "Пользователь не найден !");

            currentUser.IsBlocked = false;

            var result = await _dbContext.SaveChangesAsync();

            if (result == 0) return new Response<string>(HttpStatusCode.InternalServerError, "Аккаунт пользователя не разблокирован !");
            return new Response<string>(HttpStatusCode.OK, "Аккаунт пользователя успешно разблокирован !");
        }

        public async Task<Response<UpdateUserTypeDto>> UpdateUserTypeAsync(UpdateUserTypeDto model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            if (user == null) return new Response<UpdateUserTypeDto>(HttpStatusCode.NotFound, "Пользователь не найден !");

            user.UserType = model.UserType;

            var result = await _dbContext.SaveChangesAsync();

            if (result == 0) return new Response<UpdateUserTypeDto>(HttpStatusCode.InternalServerError, "User type not updated!");
            return new Response<UpdateUserTypeDto>(HttpStatusCode.OK, "User type successfully updated !");
        }

        public async Task<Response<string>> RemoveUserAsync(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) return new Response<string>(HttpStatusCode.NotFound, "User not found !");

            _dbContext.Users.Remove(user);
            var result = await _dbContext.SaveChangesAsync();

            if (result == 0) return new Response<string>(HttpStatusCode.InternalServerError, "User not deleted!");
            return new Response<string>(HttpStatusCode.OK, "User successfully deleted !");
        }
    }
}
