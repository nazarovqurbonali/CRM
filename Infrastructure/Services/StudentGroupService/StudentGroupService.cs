namespace Infrastructure;

public class StudentGroupService :IStudentGroupService
{
    private readonly ApplicationDbContext _context;

    public StudentGroupService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Response<List<GetStudentGroupDto>>> GetStudentGroupsAsync(PaginationFilter filter)
    {
        try
        {
            var studentGroups =  _context.StudentGroups.AsQueryable();
            var totalRecord = await studentGroups.CountAsync();
                
            var map = await studentGroups.Select(x => new GetStudentGroupDto()
            {
                Status = x.Status.ToString(),
                StudentId = x.StudentId,
                GroupId = x.GroupId,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            return new PagedResponse<List<GetStudentGroupDto>>(map,HttpStatusCode.OK, "Ok", totalRecord,
                filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new Response<List<GetStudentGroupDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetStudentGroupDto>> GetStudentGroupByIdAsync(int id)
    {
        try
        {
            var studentGroup = await _context.StudentGroups.FirstOrDefaultAsync(x => x.Id == id);
            if(studentGroup==null) return new Response<GetStudentGroupDto>(HttpStatusCode.NotFound,"StudentGroup not found"); 
            var result = new GetStudentGroupDto()
            {
                Status = studentGroup.Status.ToString(),
                StudentId = studentGroup.StudentId,
                GroupId = studentGroup.GroupId
            };
            
            return new Response<GetStudentGroupDto>(HttpStatusCode.OK,"Student group found",result);
        }
        catch (Exception e)
        {
            return new Response<GetStudentGroupDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> AddStudentGroupsAsync(AddStudentGroupDto studentGroup)
    {
        try
        {
            var map = new StudentGroup()
            {
                GroupId = studentGroup.GroupId,
                CreateDate = DateTimeOffset.Now,
                StudentId = studentGroup.StudentId,
                Status = studentGroup.Status,
                
            };

            await _context.StudentGroups.AddAsync(map);
            await _context.SaveChangesAsync();

            return new Response<string>(HttpStatusCode.OK, "StudentGroup added successfully ");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateStudentGroupsAsync(UpdateStudentGroupDto studentGroup)
    {
        try
        {
            var request = await _context.StudentGroups.FirstOrDefaultAsync(x => x.Id == studentGroup.Id);
            if (request == null) return new Response<string>(HttpStatusCode.NotFound, "StudentGroups  not found ");
            
             request.GroupId = studentGroup.GroupId;
             request.StudentId = studentGroup.StudentId;
             request.UpdateDate = DateTime.UtcNow;
             request.Status = studentGroup.Status;
             request.Id = studentGroup.Id;

             await _context.SaveChangesAsync();

             return new Response<string>(HttpStatusCode.OK, "StudentGroups updated successfully");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteStudentGroupsAsync(int id)
    {
        try
        {
            var studentGroup = await _context.StudentGroups.FirstOrDefaultAsync(x => x.Id == id);
            if(studentGroup == null) return new Response<bool>(HttpStatusCode.NotFound, "StudentGroups not found");
            _context.StudentGroups.Remove(studentGroup);
            await _context.SaveChangesAsync();

            return new Response<bool>(HttpStatusCode.OK, "StudentGroups deleted successfully");
        }
        catch (Exception e)
        {
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}