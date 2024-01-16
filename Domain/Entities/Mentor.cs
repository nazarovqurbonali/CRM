namespace Domain
{
    public class Mentor : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public Gender Gender { get; set; } = Gender.Male;
        public MentorStatus Status { get; set; } = MentorStatus.Active;
        public virtual ICollection<MentorPosition> Positions { get; set; } = null!;
    }
}
