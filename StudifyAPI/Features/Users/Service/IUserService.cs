using StudifyAPI.Features.Users.DTOs;

namespace StudifyAPI.Features.Users.Services
{
    public interface IUserService
    {
        Task<List<UserReadDTO>> GetAllUsersAsync();
        Task<UserReadDTO> GetUserByIdAsync(int id);
        Task<UserReadDTO> GetUserByEmailAsync(string email);
        Task<UserReadDTO> PatchUserAsync(int id, UserPatchDTO userPatchDTO);
        Task<UserReadDTO> DeleteUserAsync(int id);
    }
}
