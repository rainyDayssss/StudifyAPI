using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Users.Services;


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
        public async Task<IActionResult> GetAsync()
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
            return Ok(await _userService.CreateUserAsync(userCreateDTO));
        }

        // Update user partially
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> PatchAsync(int id, [FromBody] UserPatchDTO userPatchDTO) { 
            return Ok(await _userService.PatchUserAsync(id, userPatchDTO));
        }

        // Delete user
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _userService.DeleteUserAsync(id));
        }   
    }
}
