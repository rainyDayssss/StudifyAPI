
using StudifyAPI.Features.Users.Models;

namespace StudifyAPI.Features.FriendRequests.DTO
{
    public class FriendRequestReadDTO
    {
        public int Id { get; set; }
        public User Sender { get; set; } = null!;
        public User Receiver { get; set; } = null!;
    }
}
