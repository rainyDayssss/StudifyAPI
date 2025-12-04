
using StudifyAPI.Features.Users.Models;

namespace StudifyAPI.Features.FriendRequests.Model
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int SenderId { get; set; } // userId
        public int ReceiverId { get; set; } // another userId
        public User Sender { get; set; } = null!;
        public User Receiver { get; set; } = null!;

    }
}
