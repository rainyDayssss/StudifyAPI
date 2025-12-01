using System.Text.Json.Serialization;

namespace StudifyAPI.Features.Users.Models
{
    public class UserLevel
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}
