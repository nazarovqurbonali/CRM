namespace Domain
{
    public class UpdatePermissionForRoleDto
    {
        public int RoleId { get; set; }
        public string PermissionType { get; set; } = null!;
        public string PermissionValue { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}

