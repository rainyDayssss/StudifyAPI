using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Features.UserStreaks.DTO;
using StudifyAPI.Features.UserStreaks.Model;
using StudifyAPI.Features.UserStreaks.Repository;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.UserStreaks.Service
{
    public class UserStreakService : IUserStreakService
    {
        private readonly IUserStreakRepository _userStreakRepository;
        public UserStreakService(IUserStreakRepository userStreakRepository, IUserRepository userRepository)
        {
            _userStreakRepository = userStreakRepository;
        }

        public async Task<UserStreakDTO> GetUserStreakByUserIdAsync(int userId)
        {
            var streak = await _userStreakRepository.GetByUserIdAsync(userId);
            if (streak is null) {
                throw new StreakNotFoundException("Streak not found"); 
            }
            var userStreakDTO = new UserStreakDTO
            {
                CurrentStreakDays = streak.CurrentStreakDays,
                LastUpdated = streak.LastUpdated
            };
            return userStreakDTO;
        }

        public async Task<UserStreakDTO> UpdateUserStreaksAsync(int userId)
        {
            var streak = await _userStreakRepository.GetByUserIdAsync(userId);
            if (streak is null) { // This will rarely happen
                throw new StreakNotFoundException("User streak not found"); 
            }

            // map streak to streakDTO
            var streakDTO = new UserStreakDTO
            {
                CurrentStreakDays = streak.CurrentStreakDays,
                LastUpdated = streak.LastUpdated
            };

            var today = DateTime.Today;
            if (streak.LastUpdated == today)
            {
                // Already updated today, do nothing
                return streakDTO;
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

            return streakDTO;
        }
    }
}
