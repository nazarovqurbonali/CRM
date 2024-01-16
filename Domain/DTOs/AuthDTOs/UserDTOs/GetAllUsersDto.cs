namespace Domain
{
    public class GetAllUsersDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public bool IsBlocked { get; set; } = false;
        public UserType UserType { get; set; }
        public List<GetAllRolesDto> Roles { get; set; } = new();
    }
}


