using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Tasks.Repository;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.Tasks.Service
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUserTaskRepository _taskRepository;
        public UserTaskService(IUserTaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<UserTaskCreateDTO> CreateTaskAsync(int userId, UserTaskCreateDTO taskCreateDTO)
        {
            return await _taskRepository.CreateTaskAsync(userId, taskCreateDTO);
        }

        public async Task<UserTaskCreateDTO?> DeleteTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }

        public async Task<List<UserTaskCreateDTO>> GetAllTasksByUserIdAsync(int userId)
        {
            return await _taskRepository.GetAllTasksByUserIdAsync(userId);
        }

        public async Task<UserTaskCreateDTO?> GetTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }

        public async Task<UserTaskCreateDTO?> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO)
        {
            var existingTask = await _taskRepository.PatchTaskAsync(taskId, userId, taskPatchDTO);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }
    }
}
