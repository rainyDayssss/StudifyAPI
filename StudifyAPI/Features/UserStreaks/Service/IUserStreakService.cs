using StudifyAPI.Features.UserStreaks.Model;

namespace StudifyAPI.Features.UserStreaks.Service
{
    public interface IUserStreakService
    {
        public Task<UserStreak?> GetUserStreakByUserIdAsync(int userId);
        public Task<UserStreak>UpdateUserStreaksAsync(int userId);
    }
}
