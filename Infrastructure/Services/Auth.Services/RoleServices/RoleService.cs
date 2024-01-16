namespace Infrastructure
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<List<GetAllPermissionsDto>>> GetAllPermissionsByRoleIdAsync(PermissionFilter filter)
        {
            var validFilter = new PermissionFilter(filter.PageNumber, filter.PageSize);

            var role = await _dbContext.Roles.FirstOrDefaultAsync(x=>x.Id==filter.RoleId);
            if(role==null) return new PagedResponse<List<GetAllPermissionsDto>>(HttpStatusCode.NotFound,"Role not found !",1, validFilter.PageNumber, validFilter.PageSize);

            var roleClaims = await _dbContext.RoleClaims.Where(x => x.RoleId == role.Id).ToListAsync();

            var allPermissions = new List<GetAllPermissionsDto>();
            allPermissions.GetPermissions(typeof(Permissions));

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(x => x.ClaimValue == permission.PermissionValue))
                {
                    permission.IsSelected = true;
                }
            }

            if (filter.PermissionValue != null)
            {
                allPermissions = allPermissions.Where(x => x.PermissionValue.ToLower().Contains(filter.PermissionValue.ToLower())).ToList();
            }


            if (allPermissions.Count == 0) return new PagedResponse<List<GetAllPermissionsDto>>(HttpStatusCode.NoContent, "Not permissions", 1, validFilter.PageNumber, validFilter.PageSize);
            return new PagedResponse<List<GetAllPermissionsDto>>(allPermissions
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList(),HttpStatusCode.OK, "All permissions", allPermissions.Count, validFilter.PageNumber, validFilter.PageSize);
        }

        public async Task<List<GetAllRolesDto>> GetAllRolesAsync()
        {
            var roles = await _dbContext.Roles.Select(role => new GetAllRolesDto
            {
                RoleId = role.Id,
                RoleName = role.RoleName,
                IsActive = role.IsActive
            }).ToListAsync();

            return roles;
        }

        public async Task<Response<UpdatePermissionForRoleDto>> UpdateRolePermissionForRoleAsync(UpdatePermissionForRoleDto model)
        {
            try
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == model.RoleId);
                if (role == null) return new Response<UpdatePermissionForRoleDto>(HttpStatusCode.NotFound, "Role not found !");


                var roleClaim = await _dbContext.RoleClaims.Where(x => x.RoleId == role.Id &&
                x.ClaimType.ToLower() == model.PermissionType.ToLower() && x.ClaimValue.ToLower() == model.PermissionValue.ToLower()).FirstOrDefaultAsync();

                string response = string.Empty;
                if (model.IsSelected == true)
                {
                    if (roleClaim == null)
                    {
                        try
                        {
                            await _dbContext.RoleClaims.AddAsync(new RoleClaim { RoleId = role.Id, ClaimType = model.PermissionType, ClaimValue = model.PermissionValue });
                            await _dbContext.SaveChangesAsync();
                            response = "Data successfully added to role";
                        }
                        catch (Exception ex)
                        {
                            return new Response<UpdatePermissionForRoleDto>(HttpStatusCode.InternalServerError, ex.Message);
                        }

                    }
                }
                else
                {
                    if (roleClaim != null)
                    {
                        try
                        {
                            _dbContext.RoleClaims.Remove(roleClaim);
                            await _dbContext.SaveChangesAsync();
                            response = "Data successfully removed from role";
                        }
                        catch (Exception ex)
                        {
                            return new Response<UpdatePermissionForRoleDto>(HttpStatusCode.InternalServerError, ex.Message);
                        }
                    }
                }
                return new Response<UpdatePermissionForRoleDto>(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                return new Response<UpdatePermissionForRoleDto>(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
