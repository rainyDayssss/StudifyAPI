using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;

namespace StudifyAPI.Features.Tasks.Service
{
    public interface IUserTaskService
    {
        public Task<List<UserTaskCreateDTO>> GetAllTasksByUserIdAsync(int userId);
        public Task<UserTaskCreateDTO> CreateTaskAsync(int userId, UserTaskCreateDTO taskCreateDTO);
        public Task<UserTaskCreateDTO?> GetTaskAsync(int taskId, int userId);
        public Task<UserTaskCreateDTO?> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO);
        public Task<UserTaskCreateDTO?> DeleteTaskAsync(int taskId, int userId);
    }
}
