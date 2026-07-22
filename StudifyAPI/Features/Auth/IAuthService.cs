using StudifyAPI.Features.Auth.DTOs;
using StudifyAPI.Features.Users.DTOs;

namespace StudifyAPI.Features.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(UserCreateDTO userCreateDTO);
        Task<AuthResponseDTO> LoginAsync(UserLoginDTO userLoginDTO);
        Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);
        Task<UserReadDTO> LogoutAsync(int userId);
    }
}
