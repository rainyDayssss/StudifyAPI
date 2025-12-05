
using StudifyAPI.Features.Users.Models;
using System.Text.Json.Serialization;

namespace StudifyAPI.Features.FriendRequests.Model
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int SenderId { get; set; } // userId
        public int ReceiverId { get; set; } // another userId
        [JsonIgnore]
        public User Sender { get; set; } = null!;
        [JsonIgnore]
        public User Receiver { get; set; } = null!;

    }
}
