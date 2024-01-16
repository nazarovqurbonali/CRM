namespace Infrastructure.Services.GroupService;

public interface IGroupService
{
    Task<Response<List<GetGroupDto>>> GetGroupsAsync(GroupFilter filter);
    Task<Response<GetGroupDto>> GetGroupsByIdAsync(int id);
    Task<Response<string>> AddGroupAsync(AddGroupDto group);
    Task<Response<string>> UpdateGroupAsync(UpdateGroupDto group);
    Task<Response<bool>> DeleteGroupAsync(int id);
}