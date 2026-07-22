using AutoMapper;
using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Tasks.Repository;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.Tasks.Service
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUserTaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public UserTaskService(IUserTaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<UserTaskReadDTO> CreateTaskAsync(int userId, UserTaskCreateDTO taskCreateDTO)
        {
            var task = _mapper.Map<UserTask>(taskCreateDTO);
            task.IsCompleted = false;
            task.UserId = userId;

            await _taskRepository.CreateTaskAsync(task);

            return _mapper.Map<UserTaskReadDTO>(task);
        }

        public async Task<UserTaskReadDTO> DeleteTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.DeleteTaskAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return _mapper.Map<UserTaskReadDTO>(existingTask);
        }

        public async Task<List<UserTaskReadDTO>> GetAllTasksByUserIdAsync(int userId)
        {
            var tasks = await _taskRepository.GetAllTasksByUserIdAsync(userId);
            return _mapper.Map<List<UserTaskReadDTO>>(tasks);
        }

        public async Task<UserTaskReadDTO> GetTaskAsync(int taskId, int userId)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId, userId);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return _mapper.Map<UserTaskReadDTO>(existingTask);
        }

        public async Task<UserTaskReadDTO> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO)
        {
            var existingTask = await _taskRepository.PatchTaskAsync(taskId, userId, taskPatchDTO);
            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found");
            }
            return _mapper.Map<UserTaskReadDTO>(existingTask);
        }
    }
}
