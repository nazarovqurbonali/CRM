namespace Infrastructure
{
    public interface IMentorService
    {
        Task<PagedResponse<List<GetMentorDto>>> GetAllMentors(MentorFilter filter,CancellationToken token = default);
        Task<Response<GetMentorDetailDto>> GetMentorById(int mentorId, CancellationToken token = default);
        Task<Response<AddMentorDto>> AddMentor(AddMentorDto model, CancellationToken token = default);
        Task<Response<UpdateMentorDto>> UpdateMentor(UpdateMentorDto model, CancellationToken token = default);
        Task<Response<string>> DeleteMentorById(int mentorId, CancellationToken token = default);
    }
}


