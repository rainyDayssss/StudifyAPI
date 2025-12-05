using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.Friends.DTO;
using StudifyAPI.Features.Friends.Service;
using StudifyAPI.Shared;

namespace StudifyAPI.Features.Friends.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendService _friendService;
        public FriendsController(IFriendService friendService) {
            _friendService = friendService;
        }
        // Get all user's friends
        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {
            var userId = GetUserIdFromClaims();
            var friends = await _friendService.GetAllFriendsAsync(userId);
            return Ok( new ResponseDTO<List<FriendReadDTO>> { 
                Success = true,
                Message = "Retrieved user's friends succesfully.",
                Data = friends
            });
        }

        // Get user friend
        [HttpGet("{friendId}")]
        public async Task<IActionResult> GetAsync(int friendId) {
            var userId = GetUserIdFromClaims();
            var friend = await _friendService.GetFriendAsync(userId, friendId);
            return Ok( new ResponseDTO<FriendReadDTO> { 
                Success = true,
                Message = "User friend retrieved sucessfully.",
                Data = friend
            });
        }

        // Add friend
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] FriendCreateDTO createDTO)
        {
            var addedFriend = await _friendService.AddFriendAsync(createDTO);
            return Ok(new ResponseDTO<FriendReadDTO>
            { 
                Success = true,
                Message = "Friend added succesfully.",
                Data = addedFriend
            });
        }

        // Unfriend 
        [HttpDelete("{friendId}")]
        public async Task<IActionResult> DeleteAsync(int friendId) {
            var userId = GetUserIdFromClaims();
            var deletedFriend = await _friendService.DeleteFriendAsync(userId, friendId);
            return Ok(new ResponseDTO<FriendReadDTO> { 
                Success = true,
                Message = "Friend deleted (unfriended) successfully.",
                Data = deletedFriend
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
