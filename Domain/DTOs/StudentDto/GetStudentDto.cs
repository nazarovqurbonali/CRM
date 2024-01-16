namespace Domain;

public class GetStudentDto:StudentDto
{
    public int Id { get; set; }
    public string?  Gender { get; set; }
    public string? Status{ get; set; } 
    public Status? Status1{ get; set; } 
    public Gender?  Gender1 { get; set; }

}