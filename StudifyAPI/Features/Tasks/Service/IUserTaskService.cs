using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;

namespace StudifyAPI.Features.Tasks.Service
{
    public interface IUserTaskService
    {
        public Task<List<UserTaskReadDTO>> GetAllTasksByUserIdAsync(int userId);
        public Task<UserTaskReadDTO> CreateTaskAsync(int userId, UserTaskCreateDTO taskCreateDTO);
        public Task<UserTaskReadDTO> GetTaskAsync(int taskId, int userId);
        public Task<UserTaskReadDTO> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO);
        public Task<UserTaskReadDTO> DeleteTaskAsync(int taskId, int userId);
    }
}
