using System.ComponentModel.DataAnnotations;

namespace StudifyAPI.Features.Tasks.DTO
{
    public class UserTaskReadDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}
