namespace Domain
{
    public class UpdateFacultyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null;
        public FacultyStatus Status { get; set; }
    }
}

