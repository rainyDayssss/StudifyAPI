using StudifyAPI.Features.FriendRequests.Model;

namespace StudifyAPI.Features.FriendRequests.Repository
{
    public interface IFriendRequestRepository
    {
        // Send request
        public Task<FriendRequest> CreateFriendRequestAsysnc(FriendRequest friendRequest);

        // Get all sent requests for a user
        public Task<List<FriendRequest>> GetAllSentRequestsAsync(int userId);
        
        // Get all receive requests for a user
        public Task<List<FriendRequest>> GetAllReceivedRequestsAsync(int userId);

        // Cancel sent request,  senderId = logged in user
        public Task<FriendRequest?> DeleteFriendRequestAsync(int senderId, int receiverId);

        // Accept request,  receiverId = logged in user
        public Task<FriendRequest?> AcceptFriendRequestAsync(int senderId, int receiverId);
    }
}
