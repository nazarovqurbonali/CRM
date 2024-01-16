using System.Text.Json.Serialization;

namespace Domain
{
    public class UpdateCurrentUserDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
