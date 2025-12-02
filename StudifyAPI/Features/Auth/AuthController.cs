using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Users.Services;

namespace StudifyAPI.Features.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var token = await _userService.LoginAsync(userLoginDTO);
            return Ok(token);
        }
    }
}
