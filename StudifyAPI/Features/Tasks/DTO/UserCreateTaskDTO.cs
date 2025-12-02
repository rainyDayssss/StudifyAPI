namespace StudifyAPI.Features.Tasks.DTO
{
    public class UserCreateTaskDTO
    {
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
    }
}
