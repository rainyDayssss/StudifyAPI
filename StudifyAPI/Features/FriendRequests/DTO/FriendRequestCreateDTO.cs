using System.ComponentModel.DataAnnotations;

namespace StudifyAPI.Features.FriendRequests.DTO
{
    public class FriendRequestCreateDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
