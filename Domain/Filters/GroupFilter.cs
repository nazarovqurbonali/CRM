namespace Domain;

public class GroupFilter:PaginationFilter
{
    public string? GroupName { get; set; }

    public GroupFilter()
    {
        PageNumber = 1;
        PageSize = 10;
    }
    public GroupFilter(int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
    }
}
