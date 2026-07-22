using AutoMapper;
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
        private readonly IMapper _mapper;
        public UserStreakService(IUserStreakRepository userStreakRepository, IMapper mapper)
        {
            _userStreakRepository = userStreakRepository;
            _mapper = mapper;
        }

        public async Task<UserStreakReadDTO> GetUserStreakByUserIdAsync(int userId)
        {
            var streak = await _userStreakRepository.GetByUserIdAsync(userId);
            if (streak is null) {
                throw new StreakNotFoundException("Streak not found"); 
            }
            return _mapper.Map<UserStreakReadDTO>(streak);
        }
        
        // you must update this after user had spend maybe 25 mins using the app
        public async Task<UserStreakReadDTO> UpdateUserStreaksAsync(int userId)
        {
            var streak = await _userStreakRepository.GetByUserIdAsync(userId);
            if (streak is null) { // This will rarely happen
                throw new StreakNotFoundException("User streak not found"); 
            }

            var today = DateTime.Today;
            if (streak.LastUpdated == today)
            {
                // Already updated today, do nothing
                return _mapper.Map<UserStreakReadDTO>(streak);
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

            return _mapper.Map<UserStreakReadDTO>(streak);
        }
    }
}
