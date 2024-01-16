namespace Domain
{
    public class GetMentorDetailDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public Gender Gender { get; set; }
        public MentorStatus Status { get; set; }
    }
}

