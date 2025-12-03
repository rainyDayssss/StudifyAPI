using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;

namespace StudifyAPI.Features.Tasks.Repository
{
    public interface IUserTaskRepository
    {
        public Task<List<UserTask>> GetAllTasksByUserIdAsync(int userId);
        public Task<UserTask> CreateTaskAsync(int userId, UserTask task);
        public Task<UserTask?> GetTaskByIdAsync(int taskId, int userId);
        public Task<UserTask?> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO);
        public Task<UserTask?> DeleteTaskAsync(int taskId, int userId);
    }
}
