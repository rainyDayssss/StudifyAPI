using System.Text.Json.Serialization;

namespace StudifyAPI.Features.Users.Models
{
    public class UserStreak
    {
        public int Id { get; set; }
        public int CurrentStreak { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}
