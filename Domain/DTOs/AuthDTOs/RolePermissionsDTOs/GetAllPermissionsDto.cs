namespace Domain
{
    public class GetAllPermissionsDto
    {
        public string PermissionType { get; set; } = null!;
        public string PermissionValue { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}

