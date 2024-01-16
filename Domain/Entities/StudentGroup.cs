namespace Domain
{
    public class StudentGroup : BaseEntity
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;
        public int GroupId { get; set; }
        public virtual Group Group { get; set; } = null!;
        public UserGroupStatus Status { get; set; }
    }
}


