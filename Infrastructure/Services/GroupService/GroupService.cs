namespace Infrastructure.Services.GroupService;

public class GroupService: IGroupService
{
    private readonly ApplicationDbContext _context;

    public GroupService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Response<List<GetGroupDto>>> GetGroupsAsync(GroupFilter filter)
    {
        try
        {
            var query = _context.Groups.AsQueryable();
            
            if (!string.IsNullOrEmpty(filter.GroupName))
                query = query.Where(s =>
                    s.Name.ToLower().Contains(filter.GroupName.ToLower()));

            var totalRecord = query.Count();

            var groups = await query.Select(x => new GetGroupDto()
            {
                Name = x.Name,
                Id = x.Id,
                FacultyId = x.FacultyId,
                StartDate = x.StartDate,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize)
                .ToListAsync();

            return new PagedResponse<List<GetGroupDto>>(groups, HttpStatusCode.OK,"Ok", totalRecord, filter.PageNumber,
                filter.PageSize);
        }
        catch (Exception e)
        {
            return new Response<List<GetGroupDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetGroupDto>> GetGroupsByIdAsync(int id)
    {
        try
        {
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if(group==null) return new Response<GetGroupDto>(HttpStatusCode.NotFound,"Group not found");
            var result = new GetGroupDto()
            {
                Name = group.Name,
                FacultyId = group.FacultyId,
                StartDate = group.StartDate,
                Id = group.Id,
            };

            return new Response<GetGroupDto>(HttpStatusCode.OK, "Group found", result);
        }
        catch (Exception e)
        {
            return new Response<GetGroupDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddGroupAsync(AddGroupDto model)
    {
        try
        {
            var newGroup = new Group()
            {
                Name = model.Name,
                CreateDate = DateTime.UtcNow,
                FacultyId = model.FacultyId,
                StartDate = DateTime.UtcNow,
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "Successfully added group");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateGroupAsync(UpdateGroupDto group)
    {
        try
        {
           var request = await _context.Groups.FirstOrDefaultAsync(x=>x.Id==group.Id);
           if(request==null) return new Response<string>(HttpStatusCode.NotFound,"Group not found");

           request.UpdateDate = DateTime.UtcNow;
           request.Id = group.Id;
           request.FacultyId = group.FacultyId;
           request.StartDate = group.StartDate;
           request.Name = group.Name;

           await _context.SaveChangesAsync();

           return new Response<string>(HttpStatusCode.OK, "Group updated successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteGroupAsync(int id)
    {
        try
        {
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (group == null)  return new Response<bool>(HttpStatusCode.NotFound,"Group not found");
            
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            
            return new Response<bool>(HttpStatusCode.OK, "Group deleted successfully");
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}