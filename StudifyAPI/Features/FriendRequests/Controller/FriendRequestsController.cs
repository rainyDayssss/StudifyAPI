using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudifyAPI.Features.FriendRequests.DTO;
using StudifyAPI.Features.FriendRequests.Service;
using StudifyAPI.Shared;

namespace StudifyAPI.Features.FriendRequests.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestsController : ControllerBase
    {
        private readonly IFriendRequestService _requestService;
        public FriendRequestsController(IFriendRequestService requestService)
        {
            _requestService = requestService;
        }

        // Get all received requests, ReceiverId = UserId
        [HttpGet("received/me")]
        public async Task<IActionResult> GetAllReceivedRequestsAsync()
        {
            int receiverId = GetUserIdFromClaims();
            var requests = await _requestService.GetAllReceivedRequestsAsync(receiverId);
            return Ok(new ResponseDTO<List<FriendRequestReadDTO>> {
                Success = true,
                Message = "Received friend requests retrieved successfully.",
                Data = requests
            });
        }

        // Get all sent requests, SenderId = UserId logged in
        [HttpGet("sent/me")]
        public async Task<IActionResult> GetAllSentRequestsAsync()
        {
            int senderId = GetUserIdFromClaims();
            var requests = await _requestService.GetAllSentRequestsAsync(senderId);
            return Ok(new ResponseDTO<List<FriendRequestReadDTO>>
            {
                Success = true,
                Message = "Sent friend requests retrieved successfully.",
                Data = requests
            });
        }

        // Create friend request
        [HttpPost]
        public async Task<IActionResult> CreateAsync(FriendRequestCreateDTO requestDTO)
        {
            var senderId = GetUserIdFromClaims();
            var createdRequest = await _requestService.SendFriendRequestAsync(senderId, requestDTO);
            return Ok(new ResponseDTO<FriendRequestReadDTO>
            {
                Success = true,
                Message = "Friend request created and sent succesfully",
                Data = createdRequest
            });
        }

        // Accept request, receiverId = userId of logged in
        [HttpPost("{requestId}/accept")]
        public async Task<IActionResult> AcceptAsync(int requestId) {
            var userId = GetUserIdFromClaims();
            var acceptedRequest = await _requestService.AcceptFriendRequestAsync(requestId, userId);
            return Ok(new ResponseDTO<FriendRequestReadDTO>
            {
                Success = true,
                Message = "Accepted friend request successfully",
                Data = acceptedRequest
            });
        }

        // Cancel friend request sent
        [HttpDelete("{requestId}/cancel")]
        public async Task<IActionResult> CancelAsync(int requestId) {
            var userId = GetUserIdFromClaims();
            var cancelFriendRequest = await _requestService.CancelSentRequestAsync(requestId, requestId);
            return Ok( new ResponseDTO<FriendRequestReadDTO> 
            { 
                Success = true,
                Message = "Sent friend request cancelled succesfully",
                Data = cancelFriendRequest
            });
        }

        // Reject received request
        [HttpDelete("{requestId}/reject")]
        public async Task<IActionResult> RejectAsync(int requestId) {
            var userId = GetUserIdFromClaims();
            var rejectedFriendRequest = await _requestService.RejectSentRequestAsync(requestId, userId);
            return Ok( new ResponseDTO<FriendRequestReadDTO>
            {
                Success = true,
                Message = "Received request rejected successfully",
                Data = rejectedFriendRequest
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
