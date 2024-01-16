namespace Domain
{
    public class PermissionFilter : PaginationFilter
    {
        public string? PermissionValue { get; set; } = null;
        public int? RoleId { get; set; } = null;
        public PermissionFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public PermissionFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
        {
        }
    }
}