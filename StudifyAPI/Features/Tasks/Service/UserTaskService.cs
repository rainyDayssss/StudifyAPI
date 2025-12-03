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
        private readonly IUserRepository _userRepository;
        public UserTaskService(IUserTaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }
        public async Task<UserTask> CreateTaskAsync(int userId, UserTaskCreateDTO taskCreateDTO)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); // if the user was not logged in
            }

            // map DTO to Model
            var task = new UserTask
            {
                Title = taskCreateDTO.Title,
                IsCompleted = false,
                UserId = userId
            };
            return await _taskRepository.CreateTaskAsync(userId, task);
        }

        public async Task<UserTask?> DeleteTaskAsync(int taskId, int userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); // if the user was not logged in
            }

            var existingTask = await _taskRepository.DeleteTaskAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }

        public async Task<List<UserTask>> GetAllTasksByUserIdAsync(int userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); // if the user was not logged in
            }
            return await _taskRepository.GetAllTasksByUserIdAsync(userId);
        }

        public async Task<UserTask?> GetTaskAsync(int taskId, int userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); // if the user was not logged in
            }

            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }

        public async Task<UserTask?> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); // if the user was not logged in
            }

            var existingTask = await _taskRepository.PatchTaskAsync(taskId, userId, taskPatchDTO);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return existingTask;
        }
    }
}
