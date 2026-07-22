using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Auth.DTOs;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Shared;
using StudifyAPI.Shared.Extensions;

namespace StudifyAPI.Features.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserCreateDTO userCreateDTO)
        {
            var authResponse = await _authService.RegisterAsync(userCreateDTO);
            return Ok(new ResponseDTO<AuthResponseDTO>
            {
                Success = true,
                Message = "User registered and logged in successfully.",
                Data = authResponse
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDTO userLoginDTO)
        {
            var authResponse = await _authService.LoginAsync(userLoginDTO);
            return Ok(new ResponseDTO<AuthResponseDTO>
            {
                Success = true,
                Message = "Login successful.",
                Data = authResponse
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequestDTO request)
        {
            var authResponse = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(new ResponseDTO<AuthResponseDTO>
            {
                Success = true,
                Message = "Token refreshed successfully.",
                Data = authResponse
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            int userId = User.GetUserId();
            var logoutUser = await _authService.LogoutAsync(userId);

            return Ok(new ResponseDTO<UserReadDTO>
            {
                Success = true,
                Message = "Logged out successfully.",
                Data = logoutUser
            });
        }
    }
}
