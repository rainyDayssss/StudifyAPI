using StudifyAPI.Features.FriendRequests.DTO;
using StudifyAPI.Features.FriendRequests.Model;
using StudifyAPI.Features.FriendRequests.Repository;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.FriendRequests.Service
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserRepository _userRepository;
        public FriendRequestService(IFriendRequestRepository friendRequestRepository, IUserRepository userRepository)
        {
            _friendRequestRepository = friendRequestRepository;
            _userRepository = userRepository;
        }

        // Receiver = logged in user
        public Task<FriendRequestReadDTO> AcceptFriendRequestAsync(int requestId, int userId)
        {
            throw new NotImplementedException(); // needs to have the friend table first
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
                SenderFirstName = sentRequest.Sender.Firstname,
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
                SenderFirstName = fr.Sender.Firstname,
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
                SenderFirstName = fr.Sender.Firstname,
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

            if (senderId == existingReceiver.Id) 
            { 
                throw new CannotFriendYourselfException("Cannot send a friend request to yourself.");
            }

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
                ReceiverId = sentRequest.ReceiverId,
                SenderFirstName = sentRequest.Sender.Firstname,
                ReceiverFirstName = sentRequest.Receiver.Firstname
            };

            return sentRequestDTO;
        }

    }
}
