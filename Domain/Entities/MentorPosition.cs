namespace Domain
{
    public class MentorPosition : BaseEntity
    {
        public int PositionId { get; set; }
        public virtual Position Position { get; set; } = null!;
        public int MentorId { get; set; }
        public virtual Mentor Mentor { get; set; } = null!;
        public MentorPositionStatus Status { get; set; } = MentorPositionStatus.Active;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}




