using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class AddRoleToUserDto
    {
        [Required(ErrorMessage = "Enter user id")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Enter role id")]
        public int RoleId { get; set; }
    }
}
