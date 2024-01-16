namespace Domain
{
    public class GetMentorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public Gender Gender { get; set; }
        public MentorStatus Status { get; set; }
    }
}
