using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Models;

namespace StudifyAPI.Features.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsersAsync();
        public Task<User?> GetUserByIdAsync(int id);
        public Task<User> CreateUserAsync(User user);
        public Task<User?> PatchUserAsync(int id, UserPatchDTO userPatchDTO); // patch update
        public Task<User?> DeleteUserAsync(int id);
        public Task<User?> GetUserByEmailAsync(string email);
    }
}
