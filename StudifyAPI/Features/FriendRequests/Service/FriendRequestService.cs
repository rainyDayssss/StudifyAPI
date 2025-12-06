using StudifyAPI.Features.FriendRequests.DTO;
using StudifyAPI.Features.FriendRequests.Model;
using StudifyAPI.Features.FriendRequests.Repository;
using StudifyAPI.Features.Friends.DTO;
using StudifyAPI.Features.Friends.Model;
using StudifyAPI.Features.Friends.Repository;
using StudifyAPI.Features.Friends.Service;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.FriendRequests.Service
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly IFriendService _friendService;
        public FriendRequestService(IFriendRequestRepository friendRequestRepository, IUserRepository userRepository, IFriendRepository friendRepository, IFriendService friendService)
        {
            _friendRequestRepository = friendRequestRepository;
            _userRepository = userRepository;
            _friendRepository = friendRepository;
            _friendService = friendService;
        }

        // Receiver = logged in user
        public async Task<FriendRequestReadDTO> AcceptFriendRequestAsync(int requestId, int userId)
        {
            // Check if the request exist
            var receivedRequest = await _friendRequestRepository.GetFriendRequestAsync(requestId);
            if (receivedRequest is null) {
                throw new FriendRequestNotFoundException("No friend request to accept.");
            }

            // map friend request to friend
            var friendDTO = new FriendCreateDTO
            {
                UserAId = receivedRequest.ReceiverId,
                UserBId = receivedRequest.SenderId
            };

            // Add the friend to the friend table
            await _friendService.AddFriendAsync(friendDTO);

            // Delete the request from the request table
            await _friendRequestRepository.DeleteFriendRequestAsync(receivedRequest);

            // map request to reuquestDTO
            var acceptedFriendRequestDTO = new FriendRequestReadDTO
            {
                Id = receivedRequest.Id,
                ReceiverId = receivedRequest.ReceiverId,
                ReceiverFirstName = receivedRequest.Receiver.Firstname,
                SenderId = receivedRequest.SenderId,
                SenderFirstName = receivedRequest.Sender.Firstname
            };
            return acceptedFriendRequestDTO;
        }

        // Cancel sent request
        public async Task<FriendRequestReadDTO> CancelSentRequestAsync(int requestId, int userId)
        {
            var sentRequest = await _friendRequestRepository.CancelFriendRequestAsync(requestId, userId);
            if (sentRequest is null)
            {
                throw new FriendRequestNotFoundException("No sent friend request found to cancel.");
            }
            var cancelledRequest = new FriendRequestReadDTO
            {
                Id = sentRequest.Id,
                SenderId = sentRequest.SenderId,
                SenderFirstName = sentRequest.Sender.Firstname,
                ReceiverId = sentRequest.ReceiverId,
                ReceiverFirstName = sentRequest.Receiver.Firstname
            };
            return cancelledRequest;
        }

        public async Task<List<FriendRequestReadDTO>> GetAllReceivedRequestsAsync(int receiverId)
        {
            var receivedFriendRequests = await _friendRequestRepository.GetAllReceivedRequestsAsync(receiverId);
            var receivedFriendRequestDTOs = receivedFriendRequests.Select(fr => new FriendRequestReadDTO
            {
                Id = fr.Id,
                SenderId = fr.SenderId,
                SenderFirstName = fr.Sender.Firstname,
                ReceiverId = fr.ReceiverId,
                ReceiverFirstName = fr.Receiver.Firstname
            }).ToList();
            return receivedFriendRequestDTOs;
        }

        public async Task<List<FriendRequestReadDTO>> GetAllSentRequestsAsync(int senderId)
        {
            var sentFriendRequests = await _friendRequestRepository.GetAllSentRequestsAsync(senderId);
            var sentFriendRequestDTOs = sentFriendRequests.Select(fr => new FriendRequestReadDTO
            {
                Id = fr.Id,
                SenderId = fr.SenderId,
                SenderFirstName = fr.Sender.Firstname,
                ReceiverId = fr.ReceiverId,
                ReceiverFirstName = fr.Receiver.Firstname
            }).ToList();
            return sentFriendRequestDTOs;
        }

        // Reject receive request
        public async Task<FriendRequestReadDTO> RejectSentRequestAsync(int requestId, int userId)
        {
            
            var rejectedRequest = await _friendRequestRepository.RejectFriendRequestAsync(requestId, userId);
            if (rejectedRequest is null) {
                throw new FriendRequestNotFoundException("No friend request to reject");
            }
            var rejectedRequestDTO = new FriendRequestReadDTO
            {
                Id = rejectedRequest.Id,
                SenderId = rejectedRequest.SenderId,
                SenderFirstName = rejectedRequest.Sender.Firstname,
                ReceiverId = rejectedRequest.ReceiverId,
                ReceiverFirstName = rejectedRequest.Receiver.Firstname
            };
            return rejectedRequestDTO;
        }

        // Send request
        public async Task<FriendRequestReadDTO> SendFriendRequestAsync(int senderId, FriendRequestCreateDTO requestCreateDTO)
        {
            

            // check if the receiver exist by using email
            var existingReceiver = await _userRepository.GetUserByEmailAsync(requestCreateDTO.Email);
            if (existingReceiver is null)
            {
                throw new UserNotFoundException("The user you are trying to send a friend request to does not exist.");
            }

            // Check if they are already friend
            var aId = Math.Min(senderId, existingReceiver.Id);
            var bId = Math.Max(senderId, existingReceiver.Id);
            var friend = await _friendRepository.GetFriendAsync(senderId, existingReceiver.Id);
            if (friend is not null) 
            {
                throw new FriendAlreadyExistException("The friendship already exist.");
            }
            
            // check if sending request to him/her self
            if (senderId == existingReceiver.Id) 
            { 
                throw new CannotFriendYourselfException("Cannot send a friend request to yourself.");
            }

            // Get existing request in both end
            var existingRequest = await _friendRequestRepository.GetFriendRequestBetweenUsersAsync(senderId, existingReceiver.Id);
            if (existingRequest is not null)
            {
                throw new FriendRequestAlreadyExistException("A friend request between these users already exists.");
            }


            // create friend request entity from dto
            var sendFriendRequest = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = existingReceiver.Id
            };

            // Create friend request
            var sentRequest = await _friendRequestRepository.CreateFriendRequestAsync(sendFriendRequest);

            // map sent req to dto
            var sentRequestDTO = new FriendRequestReadDTO
            {
                Id = sentRequest.Id,
                SenderId = sentRequest.SenderId,
                SenderFirstName = sentRequest.Sender.Firstname,
                ReceiverId = sentRequest.ReceiverId,
                ReceiverFirstName = sentRequest.Receiver.Firstname
            };

            return sentRequestDTO;
        }

    }
}
