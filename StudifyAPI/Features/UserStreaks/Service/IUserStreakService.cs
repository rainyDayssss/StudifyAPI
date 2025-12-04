using StudifyAPI.Features.UserStreaks.DTO;
using StudifyAPI.Features.UserStreaks.Model;

namespace StudifyAPI.Features.UserStreaks.Service
{
    public interface IUserStreakService
    {
        public Task<UserStreakReadDTO> GetUserStreakByUserIdAsync(int userId);
        public Task<UserStreakReadDTO>UpdateUserStreaksAsync(int userId);
    }
}
