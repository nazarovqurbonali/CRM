namespace Domain;

public class UpdateStudentGroupDto:StudentGroupDto
{
    public int Id { get; set; }
    public UserGroupStatus Status { get; set; }

}