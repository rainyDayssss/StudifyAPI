using StudifyAPI.Features.FriendRequests.DTO;
using StudifyAPI.Features.FriendRequests.Repository;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.FriendRequests.Service
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        public FriendRequestService(IFriendRequestRepository friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }
        public Task<FriendRequestReadDTO> AcceptFriendRequestAsync(int senderId, int receiverId)
        {
            throw new NotImplementedException();
        }

            public async Task<FriendRequestReadDTO> CancelSentRequestAsync(int senderId, int receiverId)
            {
                var sentRequest = await _friendRequestRepository.DeleteFriendRequestAsync(senderId, receiverId);
                if (sentRequest is null)
                {
                    throw new FriendRequestNotFoundException("No sent friend request found to cancel.");
                }
                var cancelledRequest = new FriendRequestReadDTO
                {
                    Id = sentRequest.Id,
                    Sender = sentRequest.Sender,
                    Receiver = sentRequest.Receiver
                };
                return cancelledRequest;
            }

        public Task<List<FriendRequestReadDTO>> GetAllReceivedRequestsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<FriendRequestReadDTO>> GetAllSentRequestsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<FriendRequestReadDTO> SendFriendRequestAsync(int senderId, int receiverId)
        {
            throw new NotImplementedException();
        }
    }
}
