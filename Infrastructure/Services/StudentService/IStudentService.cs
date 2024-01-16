namespace Infrastructure;

public interface IStudentService
{
    Task<Response<List<GetStudentDto>>> GetStudentsAsync(StudentFilter filter);
    Task<Response<GetStudentDto>> GetStudentsByIdAsync(int id);
    Task<Response<string>> AddStudentAsync(AddStudentDto student);
    Task<Response<string>> UpdateStudentAsync(UpdateStudentDto student);
    Task<Response<bool>> DeleteStudentAsync(int id);
}