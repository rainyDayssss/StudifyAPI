using StudifyAPI.Features.UserStreaks.DTO;
using StudifyAPI.Features.UserStreaks.Model;

namespace StudifyAPI.Features.UserStreaks.Service
{
    public interface IUserStreakService
    {
        public Task<UserStreakDTO> GetUserStreakByUserIdAsync(int userId);
        public Task<UserStreakDTO>UpdateUserStreaksAsync(int userId);
    }
}
