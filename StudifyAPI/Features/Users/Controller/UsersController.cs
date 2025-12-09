using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Services;
using StudifyAPI.Shared;


namespace StudifyAPI.Features.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {   
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // This could be just for testing purposes, in real life we might not want to expose all users.
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(new ResponseDTO<List<UserReadDTO>>
            {
                Success = true,
                Message = "Users retrieved successfully.",
                Data = await _userService.GetAllUsersAsync()
            });
        }

        // Get Profile
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            int userId = GetUserIdFromClaims();
            var userProfile = await _userService.GetUserByIdAsync(userId);
            return Ok(new ResponseDTO<UserReadDTO>
            {
                Success = true,
                Message = "User profile retrieved successfully.",
                Data = userProfile
            });
        }
        
        // search by using email // test for the new git local account
        [HttpGet("{email}")]
        [Authorize]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(new ResponseDTO<UserReadDTO>
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = user
            });
        }

        // SignUp new user
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateDTO userCreateDTO)
        {
            var createdUser = await _userService.CreateUserAsync(userCreateDTO);
            return Ok(new ResponseDTO<UserReadDTO>
            {
                Success = true,
                Message = "User created successfully.",
                Data = createdUser
            });
        }

        // Update the logged in user partially 
        [HttpPatch("me")]
        [Authorize]
        public async Task<IActionResult> PatchAsync([FromBody] UserPatchDTO userPatchDTO) {
            foreach (var c in User.Claims)
            {
                Console.WriteLine($"{c.Type} : {c.Value}");
            }

            // Get the user ID from the JWT token claims
            int userId = GetUserIdFromClaims();
            var updatedUser = await _userService.PatchUserAsync(userId, userPatchDTO);
            return Ok(new ResponseDTO<UserReadDTO>
            { 
                Success = true,
                Message = "Your account has been updated successfully.",
                Data = updatedUser 
            });
        }

        // Delete logged in user
        [HttpDelete("me")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync()
        {

            int userId = GetUserIdFromClaims();
            var deletedUser = await _userService.DeleteUserAsync(userId);
            return Ok(new ResponseDTO<UserReadDTO> 
            {
                Success = true,
                Message = "Your account has been deleted successfully.",
                Data = deletedUser 
            });
        }

        // For logging out, let the client just delete the token on their side.
        [HttpPost("me/logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            int userId = GetUserIdFromClaims();
            var logoutUser = await _userService.LogoutAsync(userId);

            return Ok(new ResponseDTO<UserReadDTO>
            {
                Success = true,
                Message = "Logged out successfully.",
                Data = logoutUser
            });
        }

        
        private int GetUserIdFromClaims()
        {
            if (!int.TryParse(User.FindFirst("userId")?.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid user token.");
            return userId;
        }
    }
}
