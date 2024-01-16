namespace Infrastructure
{
    public class DefaultUsers : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new List<User>
            {
                 new User
                 {
                    Id=1,
                    FirstName = "Olimjon",
                    LastName ="Tajiev",
                    CreateDate = DateTimeOffset.UtcNow,
                    Email = "tajiev@gmail.com",
                    IsBlocked = false,
                    Phone = "911288822",
                    UserName ="911288822",
                    HashPassword =BCrypt.Net.BCrypt.HashPassword("911288822"),
                    UserType = UserType.Admin
                 }
            });
        }
    }
}

