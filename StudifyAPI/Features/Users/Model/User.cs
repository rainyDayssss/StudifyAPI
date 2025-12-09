
using System.ComponentModel.DataAnnotations;
using StudifyAPI.Features.FriendRequests.Model;
using StudifyAPI.Features.Friends.Model;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.UserStreaks.Model;
namespace StudifyAPI.Features.Users.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public bool IsOnline { get; set; }
        public int NumberOfFriends { get; set; } = 0;
        public UserStreak Streak { get; set; } = null!;
        public List<UserTask> Tasks { get; set; } = new();  
        public List<FriendRequest> SentFriendRequests { get; set; } = new();
        public List<FriendRequest> ReceivedFriendRequests { get; set; } = new();
        public List<Friend> FriendsAsUserA { get; set; } = new();
        public List<Friend> FriendsAsUserB { get; set; } = new();

    }
}
