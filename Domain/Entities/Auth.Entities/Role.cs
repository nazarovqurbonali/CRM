namespace Domain
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public virtual ICollection<UserRole> Users { get; set; } = null!;
        public virtual ICollection<RoleClaim> RoleClaims { get; set; } = null!;
    }
}
