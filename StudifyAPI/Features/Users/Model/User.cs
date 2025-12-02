
using System.ComponentModel.DataAnnotations;
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
        public UserStreak Streak { get; set; } = null!;
        public List<UserTask> Tasks { get; set; } = new();  
    }
}
