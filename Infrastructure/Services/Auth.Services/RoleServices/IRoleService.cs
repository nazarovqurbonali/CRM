namespace Infrastructure
{
    public interface IRoleService
    {
        Task<List<GetAllRolesDto>> GetAllRolesAsync();
        Task<Response<UpdatePermissionForRoleDto>> UpdateRolePermissionForRoleAsync(UpdatePermissionForRoleDto model);
        Task<PagedResponse<List<GetAllPermissionsDto>>> GetAllPermissionsByRoleIdAsync(PermissionFilter filter);
    }
}

