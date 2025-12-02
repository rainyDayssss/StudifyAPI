using StudifyAPI.Features.UserStreaks.Model;

namespace StudifyAPI.Features.UserStreaks.Repository
{
    public interface IUserStreakRepository
    {
        Task<UserStreak?> GetByUserIdAsync(int userId);
        Task SaveChangesAsync();
    }
}
