using StudifyAPI.Features.Users.Models;
using System.Text.Json.Serialization;

namespace StudifyAPI.Features.UserStreaks.Model
{
    public class UserStreak
    {
        public int UserId { get; set; }
        // Days in current streak
        public int CurrentStreakDays { get; set; } = 0;

        // Whn the streak was last updated
        public DateTime LastUpdated { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}
