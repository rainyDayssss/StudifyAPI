using StudifyAPI.Features.FriendRequests.DTO;
using StudifyAPI.Features.FriendRequests.Model;

namespace StudifyAPI.Features.FriendRequests.Service
{
    public interface IFriendRequestService
    {
        public Task<List<FriendRequestReadDTO>> GetAllSentRequestsAsync(int senderId);
        public Task<List<FriendRequestReadDTO>> GetAllReceivedRequestsAsync(int receiverId);
        public Task<FriendRequestReadDTO> SendFriendRequestAsync(int senderId, FriendRequestCreateDTO requestDTO);
        public Task<FriendRequestReadDTO> CancelSentRequestAsync(int requestId, int userId);
        public Task<FriendRequestReadDTO> RejectSentRequestAsync(int requestId, int userId);
        public Task<FriendRequestReadDTO> AcceptFriendRequestAsync(int requestId, int userId);
    }
}
