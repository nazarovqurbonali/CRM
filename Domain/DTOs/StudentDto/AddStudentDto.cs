namespace Domain;

public class AddStudentDto:StudentDto
{
    public Gender  Gender { get; set; }
    public Status Status{ get; set; } 
}