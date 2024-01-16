namespace Domain;

public class Student :BaseEntity
{
    public int  Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public Gender Gender { get; set; }
    public string BirthDate { get; set; } = null!;
    public Status Status{ get; set; }
    public string Password { get; set; } = null!;
    public virtual ICollection<StudentGroup> StudentGroups { get; set; } = null!;
    
}