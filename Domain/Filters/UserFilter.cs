namespace Domain
{
    public class UserFilter : PaginationFilter
    {
        public string? FirstNameOrLastName { get; set; } = null;
        public UserFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public UserFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
        {
        }
    }
}
