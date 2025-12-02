using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using StudifyAPI.Features.Users.Models;

namespace StudifyAPI.Features.Tasks.Model
{
    public class UserTaskCreateDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public int UserId { get; set;}
        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}
