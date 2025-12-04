using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Tasks.Repository;
using StudifyAPI.Features.Users.Repositories;
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
        public async Task<UserTask> CreateTaskAsync(int userId, UserTaskCreateDTO taskCreateDTO)
        {
            // map DTO to Model
            var task = new UserTask
            {
                Title = taskCreateDTO.Title,
                IsCompleted = false,        
                UserId = userId
            };
            return await _taskRepository.CreateTaskAsync(task);
        }

        public async Task<UserTask?> DeleteTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.DeleteTaskAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }

        public async Task<List<UserTask>> GetAllTasksByUserIdAsync(int userId)
        {
            return await _taskRepository.GetAllTasksByUserIdAsync(userId);
        }

        public async Task<UserTask?> GetTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }

        public async Task<UserTask?> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO)
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
