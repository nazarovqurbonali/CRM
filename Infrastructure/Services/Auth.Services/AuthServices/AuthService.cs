using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string secretKey; 
        public AuthService(ApplicationDbContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
             secretKey = configuration.GetSection("SecretKey:Key").Value??Guid.NewGuid().ToString();
        }
        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == request.PhoneNumber.ToLower().Trim());
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashPassword)) return new Response<LoginResponse>(HttpStatusCode.Unauthorized, "Логин или пароль не правильный!");

            if(user.IsBlocked==true) return new Response<LoginResponse>(HttpStatusCode.BadRequest, "Ваш пароль заблокирован обратитесь k своему преподавателю!");

            var response = await GenerateJWT_Token(user);

            return new Response<LoginResponse>(HttpStatusCode.OK, "Вход успешно", response);
        }

        public async Task<Response<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == request.UserName.ToLower().Trim());
            if (user != null) return new Response<RegisterResponse>(HttpStatusCode.Found, "Этот логин занят придумайте другое названия пожалуйста");

            if(request.Password.Length < 4 || request.Password.Length > 200) return new Response<RegisterResponse>(HttpStatusCode.NotFound, "Пароль должен содержать от 4 до 200 символов");

            if (request.UserName.Length < 4 || request.UserName.Length > 200) return new Response<RegisterResponse>(HttpStatusCode.NotFound, "Логин должен содержать от 4 до 200 символов");

            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                CreateDate = DateTimeOffset.UtcNow,
                Email = request.Email,
                IsBlocked = false,
                Phone = request.Phone,
                HashPassword=BCrypt.Net.BCrypt.HashPassword(request.Password),
                UserType = UserType.User
            };

            await _dbContext.Users.AddAsync(newUser);
            var result = await _dbContext.SaveChangesAsync();

            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == Roles.User);
            if (role != null)
            {
                var roleToUser = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = role.Id,
                    CreateDate = DateTimeOffset.UtcNow
                };
                await _dbContext.UserRoles.AddAsync(roleToUser);
                result = await _dbContext.SaveChangesAsync();
            }
            if (result < 0) return new Response<RegisterResponse>(HttpStatusCode.Unauthorized, "Пользователь не авторизован !");
            return new Response<RegisterResponse>(HttpStatusCode.OK, "Пользователь успешно авторизован !");
        }
        private async Task<LoginResponse> GenerateJWT_Token(User user)
        {
            var userRoles = await _dbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x=>x.Role).ToListAsync();

            var claims = new List<Claim>
            {
                new Claim("UserId",user.Id.ToString()),
                new Claim("UserName",user.UserName),
                new Claim("Email",user.Email),
                new Claim("Phone",user.Phone)
            };

            //claims.AddRange(userRoles.Select(role => new Claim("Roles", role)));
            foreach (var role in userRoles)
            {
                claims.Add(new Claim("Roles", role.RoleName));

                var roleClaims = await _dbContext.RoleClaims.Where(x=>x.RoleId==role.Id).ToListAsync();
                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
                }
            }


            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var createToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            var writeToken = jwtTokenHandler.WriteToken(createToken);

            var response = new LoginResponse
            {
                JWT_Token = writeToken
            };
            return response;
        }
    }
}
