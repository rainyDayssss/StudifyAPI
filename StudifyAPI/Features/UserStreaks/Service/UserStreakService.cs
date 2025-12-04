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
        private readonly IUserRepository _userRepository;
        public UserStreakService(IUserStreakRepository userStreakRepository, IUserRepository userRepository)
        {
            _userStreakRepository = userStreakRepository;
            _userRepository = userRepository;
        }

        public async Task<UserStreakDTO> GetUserStreakByUserIdAsync(int userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); // its better to check the user on user repo, then if not found, meaning there is also no streak
            }

            var streak = await _userStreakRepository.GetByUserIdAsync(userId);
            if (streak is null) {
                throw new StreakNotFoundException("Streak not found"); //TODO: a custom exception can be created here 
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
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found"); 
            }

            var streak = await _userStreakRepository.GetByUserIdAsync(userId);
            if (streak is null) { // This will rarely happen
                throw new StreakNotFoundException("User streak not found"); // TODO: create custom exception
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
