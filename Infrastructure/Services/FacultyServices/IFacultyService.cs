namespace Infrastructure
{
    public interface IFacultyService
    {
        Task<PagedResponse<List<GetFacultyDto>>> GetAllFaculties(FacultyFilter filter,CancellationToken token = default);
        Task<Response<GetFacultyDto>> GetFacultyById(int facultyId,CancellationToken token = default);
        Task<Response<AddFacultyDto>> AddFaculty(AddFacultyDto model, CancellationToken token = default);
        Task<Response<UpdateFacultyDto>> UpdateFaculty(UpdateFacultyDto model, CancellationToken token = default);
        Task<Response<string>> DeleteFacultyById(int facultyId, CancellationToken token = default);
    }
}


