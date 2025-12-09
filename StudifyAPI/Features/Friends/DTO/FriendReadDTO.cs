using StudifyAPI.Features.Users.DTOs;

namespace StudifyAPI.Features.Friends.DTO
{
    public class FriendReadDTO
    {
        public int FriendId { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public bool IsOnline { get; set; } = false;
    }
}
