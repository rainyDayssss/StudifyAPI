using StudifyAPI.Features.FriendRequests.DTO;

namespace StudifyAPI.Features.FriendRequests.Service
{
    public interface IFriendRequestService
    {
        public Task<FriendRequestReadDTO> SendFriendRequestAsync(int senderId, int receiverId);
        public Task<List<FriendRequestReadDTO>> GetAllSentRequestsAsync(int userId);
        public Task<List<FriendRequestReadDTO>> GetAllReceivedRequestsAsync(int userId);
        public Task<FriendRequestReadDTO> CancelSentRequestAsync(int senderId, int receiverId);
        public Task<FriendRequestReadDTO> AcceptFriendRequestAsync(int senderId, int receiverId);
    }
}
