namespace Domain;

public class GroupDto
{
    public string Name { get; set; } = null!;
    public DateTimeOffset StartDate { get; set; }
    public int FacultyId { get; set; }
}