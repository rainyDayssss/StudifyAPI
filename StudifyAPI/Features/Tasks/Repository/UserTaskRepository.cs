using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Shared.Database;
using StudifyAPI.Shared.Exceptions;
using System.Threading.Tasks;

namespace StudifyAPI.Features.Tasks.Repository
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly StudifyDbContext _context;
        public UserTaskRepository(StudifyDbContext context)
        {
            _context = context;
        }
        public async Task<UserTask> CreateTaskAsync(int userId, UserTask task)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); // if the user was not logged in
            }

            await _context.UserTasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
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
            return await _context.UserTasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
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
            await _context.SaveChangesAsync();
            return existingTask;
        }
    }
}
