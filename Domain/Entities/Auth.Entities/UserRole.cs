namespace Domain
{
    public class UserRole : BaseEntity
    {
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}

