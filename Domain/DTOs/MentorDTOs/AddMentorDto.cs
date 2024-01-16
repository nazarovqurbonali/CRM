namespace Domain
{
    public class AddMentorDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public IFormFile? FileName { get; set; } = null;
        public Gender Gender { get; set; } = Gender.Male;
        public MentorStatus Status { get; set; } = MentorStatus.Active;
    }
}
