using StudifyAPI.Features.UserStreaks.Model;
using StudifyAPI.Features.UserStreaks.Repository;

namespace StudifyAPI.Features.UserStreaks.Service
{
    public class UserStreakService : IUserStreakService
    {
        private readonly IUserStreakRepository _userStreakRepository;
        public UserStreakService(IUserStreakRepository userStreakRepository)
        {
            _userStreakRepository = userStreakRepository;
        }

        public async Task<UserStreak?> GetUserStreakByUserIdAsync(int userId)
        {
            return await _userStreakRepository.GetByUserIdAsync(userId);
        }

        public async Task<UserStreak> UpdateUserStreaksAsync(int userId)
        {
            var streak = await _userStreakRepository.GetByUserIdAsync(userId);

            if (streak is null) { // This will rarely happen
                throw new Exception("User streak not found"); // TODO: create custom exception
            }

            var today = DateTime.Today;
            if (streak.LastUpdated == today)
            {
                // Already updated today, do nothing
                return streak;
            }

            if (streak.LastUpdated == today.AddDays(-1))
            {
                // Continue streak
                streak.CurrentStreakDays++;
            }
            else
            {
                // Reset streak
                streak.CurrentStreakDays = 1;
            }

            streak.LastUpdated = today;

            // Automatically save changes
            await _userStreakRepository.SaveChangesAsync();

            return streak;
        }
    }
}
