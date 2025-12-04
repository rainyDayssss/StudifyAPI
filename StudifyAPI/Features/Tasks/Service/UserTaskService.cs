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
        public async Task<UserTaskReadDTO> CreateTaskAsync(int userId, UserTaskCreateDTO taskCreateDTO)
        {
            // map DTO to Model
            var task = new UserTask
            {
                Title = taskCreateDTO.Title,
                IsCompleted = false,        
                UserId = userId
            };

            await _taskRepository.CreateTaskAsync(task);

            // map Model to ReadDTO
            return new UserTaskReadDTO
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };

        }

        public async Task<UserTaskReadDTO> DeleteTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.DeleteTaskAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return new UserTaskReadDTO 
            { 
                Id = existingTask.Id,
                Title = existingTask.Title,
                IsCompleted = existingTask.IsCompleted
            };
        }

        public async Task<List<UserTaskReadDTO>> GetAllTasksByUserIdAsync(int userId)
        {
            var tasks = await _taskRepository.GetAllTasksByUserIdAsync(userId);
            var taskDTOs = tasks.Select(task => new UserTaskReadDTO { 
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            }).ToList();
            return taskDTOs;
        }

        public async Task<UserTaskReadDTO> GetTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return new UserTaskReadDTO 
            { 
                Id = existingTask.Id,
                Title = existingTask.Title,
                IsCompleted = existingTask.IsCompleted
            };
        }

        public async Task<UserTaskReadDTO> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO)
        {
            var existingTask = await _taskRepository.PatchTaskAsync(taskId, userId, taskPatchDTO);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return new UserTaskReadDTO 
            { 
                Id = existingTask.Id,
                Title = existingTask.Title,
                IsCompleted = existingTask.IsCompleted
            };
        }
    }
}
