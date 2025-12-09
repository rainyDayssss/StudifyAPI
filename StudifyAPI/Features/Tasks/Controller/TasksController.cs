using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Tasks.Service;
using StudifyAPI.Shared;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudifyAPI.Features.Tasks.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUserTaskService _taskService;
        public TasksController(IUserTaskService taskService)
        {
            _taskService = taskService;
        }

        // Get all  user's tasks
        [HttpGet("me")]
        public async Task<IActionResult> GetAllAsync()
        {
            var userId = GetUserIdFromClaims();
            return Ok(new ResponseDTO<List<UserTaskReadDTO>>
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Data = await _taskService.GetAllTasksByUserIdAsync(userId)
            });
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetAsync(int taskId)
        {
            var userId = GetUserIdFromClaims();
            return Ok(new ResponseDTO<UserTaskReadDTO?>
            {
                Success = true,
                Message = "Task retrieved successfully",
                Data = await _taskService.GetTaskAsync(taskId, userId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserTaskCreateDTO taskCreateDTO)
        {
            var userId = GetUserIdFromClaims();
            var createdTask = await _taskService.CreateTaskAsync(userId, taskCreateDTO);
            return Ok(new ResponseDTO<UserTaskReadDTO?> 
            { 
                Success = true, 
                Message = "Task created successfully", 
                Data = createdTask 
            });
        }

        [HttpPatch("{taskId}")]
        public async Task<IActionResult> PatchAsync(int taskId, [FromBody] UserTaskPatchDTO taskPatchDTO)
        {
            var userId = GetUserIdFromClaims();
            var patchedTask = await _taskService.PatchTaskAsync(taskId, userId, taskPatchDTO);
            return Ok(new ResponseDTO<UserTaskReadDTO?>
            {
                Success = true,
                Message = "Task patched successfully",
                Data = patchedTask
            });
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteAsync(int taskId)
        {
            var userId = GetUserIdFromClaims();
            var deletedTask = await _taskService.DeleteTaskAsync(taskId, userId);
            return Ok(new ResponseDTO<UserTaskReadDTO?>
            {
                Success = true,
                Message = "Task deleted successfully",
                Data = deletedTask
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
