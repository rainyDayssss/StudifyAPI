using StudifyAPI.Features.Users.Models;
using System.Text.Json.Serialization;

namespace StudifyAPI.Features.Friends.Model
{
    public class Friend
    {
        public int UserAId { get; set; }
        [JsonIgnore]
        public User UserA { set; get; } = null!;
        public int UserBId { get; set; }
        [JsonIgnore]
        public User UserB { set; get; } = null!;
    }
}
