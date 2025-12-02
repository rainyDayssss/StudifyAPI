
using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.UserStreaks.Model;
using StudifyAPI.Shared.Database;

namespace StudifyAPI.Features.UserStreaks.Repository
{
    public class UserStreakRepository : IUserStreakRepository
    {
        private readonly StudifyDbContext _context;

        public UserStreakRepository(StudifyDbContext context)
        {
            _context = context;
        }
        public async Task<UserStreak?> GetByUserIdAsync(int userId)
        {
            return await _context.UserStreaks
               .Include(s => s.User) // optional: include navigation property
               .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
