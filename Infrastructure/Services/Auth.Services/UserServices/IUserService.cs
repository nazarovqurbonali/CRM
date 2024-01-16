namespace Infrastructure
{
    public interface IUserService
    {
        Task<PagedResponse<List<GetAllUsersDto>>> GetAllUsersAsync(UserFilter filter);
        Task<Response<AddRoleToUserDto>> AddRoleToUserAsync(AddRoleToUserDto model);
        Task<Response<RemoveRoleFromUserDto>> RemoveRoleFromUserAsync(RemoveRoleFromUserDto model);
        Task<Response<UpdateCurrentUserDto>> UpdateCurrentUserInfoAsync(UpdateCurrentUserDto model);
        Task<Response<GetCurrentUserDto>> GetCurrentUserInfoAsync(int userId);
        Task<Response<UpdateUserTypeDto>> UpdateUserTypeAsync(UpdateUserTypeDto model);
        Task<Response<string>> BlockedUserAccountAsync(int userId);
        Task<Response<string>> UnBlockedUserAccountAsync(int userId);
        Task<Response<string>> RemoveUserAsync(int userId);
    }
}


