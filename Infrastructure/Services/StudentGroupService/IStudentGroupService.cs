namespace Infrastructure;

public interface IStudentGroupService
{
    Task<Response<List<GetStudentGroupDto>>> GetStudentGroupsAsync(PaginationFilter filter);
    Task<Response<GetStudentGroupDto>> GetStudentGroupByIdAsync(int id);
    Task<Response<string>> AddStudentGroupsAsync(AddStudentGroupDto studentGroup);
    Task<Response<string>> UpdateStudentGroupsAsync(UpdateStudentGroupDto studentGroup);
    Task<Response<bool>> DeleteStudentGroupsAsync(int id);
}