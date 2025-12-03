using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Tasks.DTO;
using StudifyAPI.Features.Tasks.Model;
using StudifyAPI.Features.Tasks.Service;
using StudifyAPI.Shared;
using System.Security.Claims;

namespace StudifyAPI.Features.Tasks.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IUserTaskService _taskService;
        public TasksController(IUserTaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            var userId = GetUserIdFromClaims();
            return Ok(new ResponseDTO<List<UserTask>>
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Data = await _taskService.GetAllTasksByUserIdAsync(userId)
            });
        }

        [HttpGet("{taskId}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(int taskId)
        {
            var userId = GetUserIdFromClaims();
            return Ok(new ResponseDTO<UserTask?>
            {
                Success = true,
                Message = "Task retrieved successfully",
                Data = await _taskService.GetTaskAsync(taskId, userId)
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] UserTaskCreateDTO taskCreateDTO)
        {
            var userId = GetUserIdFromClaims();
            var createdTask = await _taskService.CreateTaskAsync(userId, taskCreateDTO);
            return CreatedAtAction(
                actionName: "Get",
                routeValues: new { taskId = createdTask.Id },
                value: new ResponseDTO<UserTask>
                {
                    Success = true,
                    Message = "Task created successfully.",
                    Data = createdTask
                });
        }

        [HttpPatch("{taskId}")]
        [Authorize]
        public async Task<IActionResult> PatchAsync(int taskId, [FromBody] UserTaskPatchDTO taskPatchDTO)
        {
            var userId = GetUserIdFromClaims();
            var patchedTask = await _taskService.PatchTaskAsync(taskId, userId, taskPatchDTO);
            return Ok(new ResponseDTO<UserTask?>
            {
                Success = true,
                Message = "Task patched successfully",
                Data = patchedTask
            });
        }

        [HttpDelete("{taskId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int taskId)
        {
            var userId = GetUserIdFromClaims();
            var deletedTask = await _taskService.DeleteTaskAsync(taskId, userId);
            return Ok(new ResponseDTO<UserTask?>
            {
                Success = true,
                Message = "Task deleted successfully",
                Data = deletedTask
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
