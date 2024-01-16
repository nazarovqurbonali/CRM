namespace Domain
{
    public class Group : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTimeOffset StartDate { get; set; }
        public int FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; } = null!;
        public virtual ICollection<StudentGroup> StudentGroups { get; set; } = null!;
    }
}







