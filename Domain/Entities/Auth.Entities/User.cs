namespace Domain
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string HashPassword { get; set; } = null!;
        public bool IsBlocked { get; set; } = false;
        public UserType UserType { get; set; } = UserType.User;
        public string? ImageName { get; set; } = null;
        public virtual ICollection<UserRole> Roles { get; set; } = null!;
    }
}




