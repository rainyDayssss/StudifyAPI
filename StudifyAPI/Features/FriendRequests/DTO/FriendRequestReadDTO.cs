
using StudifyAPI.Features.Users.DTOs;


namespace StudifyAPI.Features.FriendRequests.DTO
{
    public class FriendRequestReadDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; } // userId
        public int ReceiverId { get; set; } // another userId
        public string SenderFirstName { get; set; } = null!;
        public string SenderLastName { get; set; } = null!;
        public string ReceiverFirstName { get; set; } = null!;
        
    }
}
