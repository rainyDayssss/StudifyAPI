using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Shared.Database;

namespace StudifyAPI.Features.Tasks.Repository
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly StudifyDbContext _context;
        public UserTaskRepository(StudifyDbContext context)
        {
            _context = context;
        }
        public async Task<UserTask> CreateTaskAsync(int userId, UserTask taskCreateDTO)
        {
            taskCreateDTO.UserId = userId;
            await _context.UserTasks.AddAsync(taskCreateDTO);
            await _context.SaveChangesAsync();
            return taskCreateDTO;
        }

        public async Task<UserTask?> DeleteTaskAsync(int taskId, int userId)
        {
            var existingTask = await _context.UserTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (existingTask == null)
            {
                return null;
            }
            _context.UserTasks.Remove(existingTask);
            await _context.SaveChangesAsync();
            return existingTask;
        }

        public async Task<List<UserTask>> GetAllTasksByUserIdAsync(int userId)
        {
            return await _context.UserTasks.ToListAsync();
        }

        public async Task<UserTask?> GetTaskByIdAsync(int taskId, int userId)
        {
            return await _context.UserTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        }

        public async Task<UserTask?> PatchTaskAsync(int taskId, int userId, UserTaskPatchDTO taskPatchDTO)
        {
            var existingTask = await _context.UserTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (existingTask == null)
            {
                return null;
            }
            existingTask.Title = taskPatchDTO.Title ?? existingTask.Title;
            existingTask.IsCompleted = taskPatchDTO.IsCompleted ?? existingTask.IsCompleted;
            return existingTask;
        }
    }
}
