using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.UserStreaks.DTO;
using StudifyAPI.Features.UserStreaks.Model;
using StudifyAPI.Features.UserStreaks.Service;
using StudifyAPI.Shared;

namespace StudifyAPI.Features.UserStreaks.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreaksController : ControllerBase
    {
        private readonly IUserStreakService _streakService;
        public StreaksController(IUserStreakService streakService)
        {
            _streakService = streakService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            var userId = GetUserIdFromClaims();
            return Ok(new ResponseDTO<UserStreakDTO>
            {
                Success = true,
                Message = "User streak retrieved successfully",
                Data = await _streakService.GetUserStreakByUserIdAsync(userId)
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateAsync()
        {
            var userId = GetUserIdFromClaims();

            return Ok( new ResponseDTO<UserStreakDTO> { 
                Success = true,
                Message = "User streak updated successfully",
                Data = await _streakService.UpdateUserStreaksAsync(userId)
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
