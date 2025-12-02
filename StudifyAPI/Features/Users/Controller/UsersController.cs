using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Services;
using StudifyAPI.Shared;
using System.Security.Claims;


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
        // This could be just for testing purposes
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        // This also could be just for testing 
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }
        


        // SignUp new user
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateDTO userCreateDTO)
        {
            var createdUser = await _userService.CreateUserAsync(userCreateDTO);
            return CreatedAtAction(
                actionName: "Get", 
                routeValues: new { id = createdUser.Id },
                value: new ResponseDTO<UserReadDTO> 
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
        public async Task<IActionResult> Delete()
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

        private int GetUserIdFromClaims()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(claim))
                throw new UnauthorizedAccessException("User ID claim missing in token.");

            if (!int.TryParse(claim, out var userId))
                throw new UnauthorizedAccessException("User ID claim is not a valid integer.");

            return userId;
        }
    }
}
