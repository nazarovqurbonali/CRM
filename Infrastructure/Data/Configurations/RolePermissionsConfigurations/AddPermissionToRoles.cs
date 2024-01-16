namespace Infrastructure
{
    public class AddPermissionsToRole
    {
        public static async Task AddPermissionsToRoles(ApplicationDbContext dbContext)
        {

            var roles = new List<string> {Roles.Administrator};
            foreach (var role in roles)
            {
                var model = await dbContext.Roles.FirstOrDefaultAsync(x=>x.RoleName==role);
                int roleId = 0;
                if(model==null)
                {
                    var newRole = new Role { RoleName = role, IsActive = true, CreateDate = DateTimeOffset.UtcNow };
                    await dbContext.Roles.AddAsync(newRole);
                    await dbContext.SaveChangesAsync();
                    roleId = newRole.Id;
                }
                else
                {
                    roleId = model.Id;
                }

                var permissions = new List<GetAllPermissionsDto>();
                permissions.GetPermissions(typeof(Permissions));

                if (permissions.Count != 0)
                {
                    var roleClaims = new List<RoleClaim>();
                    foreach (var permission in permissions)
                    {
                        var roleClaim = await dbContext.RoleClaims.FirstOrDefaultAsync(x=>x.RoleId==roleId&& x.ClaimType==permission.PermissionType && x.ClaimValue==permission.PermissionValue);
                        if (roleClaim != null) continue;
                        roleClaims.Add(new RoleClaim { RoleId = roleId, ClaimType = permission.PermissionType, ClaimValue = permission.PermissionValue });
                    }
                    await dbContext.RoleClaims.AddRangeAsync(roleClaims);
                    await dbContext.SaveChangesAsync();
                }

            }
        }
    }
    public class Roles
    {
        public const string Administrator = "Administrator";
        public const string User = "User";
        public const string Student = "Student";
    }
}
