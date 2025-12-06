using StudifyAPI.Features.FriendRequests.Model;

namespace StudifyAPI.Features.FriendRequests.Repository
{
    public interface IFriendRequestRepository
    {
        // Send request
        public Task<FriendRequest> CreateFriendRequestAsync(FriendRequest friendRequest);
        // Get a specific sent request between two users
        public Task<FriendRequest?> GetFriendRequestBetweenUsersAsync(int senderId, int receiverId);

        // Get all sent requests for a user
        public Task<List<FriendRequest>> GetAllSentRequestsAsync(int senderId);
        
        // Get all receive requests for a user
        public Task<List<FriendRequest>> GetAllReceivedRequestsAsync(int receiverId);

        // Cancel sent request
        public Task<FriendRequest?> CancelFriendRequestAsync(int requestId, int userId);

        // Reject received request
        public Task<FriendRequest?> RejectFriendRequestAsync(int requestId, int userId);

        // Get by request id
        public Task<FriendRequest?> GetFriendRequestAsync(int requestId);

        // Delete by request id
        public Task<FriendRequest?> DeleteFriendRequestAsync(FriendRequest friendRequest);

    }
}
