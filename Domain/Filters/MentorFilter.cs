namespace Domain
{
    public class MentorFilter : PaginationFilter
    {
        public string? Name { get; set; } = null;
        public string? Phone { get; set; } = null;
        public MentorFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public MentorFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
        {
        }
    }
}

