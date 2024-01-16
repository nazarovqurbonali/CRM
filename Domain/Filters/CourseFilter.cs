namespace Domain
{
    public class CourseFilter : PaginationFilter
    {
        public string? Name { get; set; } = null;
        public CourseFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public CourseFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
        {
        }
    }
}
