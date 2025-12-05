using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Pomodoro.Enum;
using StudifyAPI.Features.Pomodoro.Service;
using StudifyAPI.Shared;

namespace StudifyAPI.Features.Pomodoro.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PomodorosController : ControllerBase
    {
        private readonly IPomodoroService _pomodoroService;
        public PomodorosController(IPomodoroService pomodoroService)
        {
            _pomodoroService = pomodoroService;
        }

        // Get the three default pomodoros
        [HttpGet]
        public  IActionResult GetDefault()
        {
            return Ok (new ResponseDTO<List<PomodoroDTO>>
            {
                Success = true,
                Message = "Default pomodoros retrieved successfully.",
                Data = _pomodoroService.GetDefaultPomodoros()
            });
        }
    }
}
