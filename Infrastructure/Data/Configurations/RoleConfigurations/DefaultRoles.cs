namespace Infrastructure
{
    public class DefaultRoles : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new List<Role>
            {
                new Role{Id=1,RoleName=Roles.Administrator.ToString(),IsActive=true},
                new Role{Id=2,RoleName=Roles.User.ToString(),IsActive=true},
                new Role{Id=3,RoleName=Roles.Student.ToString(),IsActive=true},
            });
        }
    }    
}
