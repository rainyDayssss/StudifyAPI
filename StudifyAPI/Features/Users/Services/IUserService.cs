using StudifyAPI.Features.Auth;
using System.Globalization;

namespace StudifyAPI.Features.Users.Services
{
    public interface IUserService
    {
        public Task<List<UserReadDTO>> GetAllUsersAsync();
        public Task<UserReadDTO> GetUserByIdAsync(int id);
        public Task<UserReadDTO> CreateUserAsync(UserCreateDTO userCreateDTO);
        public Task<UserReadDTO> PatchUserAsync(int id, UserPatchDTO userPatchDTO);
        public Task<UserReadDTO> DeleteUserAsync(int id);
        public Task<UserReadDTO> GetUserByEmailAsync(string email);
        public Task<string> LoginAsync(UserLoginDTO userLoginDTO);
    }
}
