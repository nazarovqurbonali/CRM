namespace Domain
{
    public class FacultyFilter : PaginationFilter
    {
        public string? Name { get; set; }

        public FacultyFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public FacultyFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
        {
        }
    }
}

