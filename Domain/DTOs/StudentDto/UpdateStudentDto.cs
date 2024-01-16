namespace Domain;

public class UpdateStudentDto:StudentDto
{
    public int Id { get; set; }
    public Gender Gender { get; set; }
    public Status Status{ get; set; } 
}