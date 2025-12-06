using StudifyAPI.Features.Friends.DTO;
using StudifyAPI.Features.Friends.Model;
using StudifyAPI.Features.Friends.Repository;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.Friends.Service
{
    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        public FriendService(IFriendRepository friendRepository) {
            _friendRepository = friendRepository;
        }
        public async Task<FriendReadDTO> AddFriendAsync(FriendCreateDTO createDTO)
        {
            var friend = new Friend
            {
                UserAId = Math.Min(createDTO.UserAId, createDTO.UserBId),
                UserBId = Math.Max(createDTO.UserAId, createDTO.UserBId)
            };
            // Get the created friend
            var createdFriend = await _friendRepository.AddFriendAsync(friend);

            if (createdFriend is null) // TODO: this should be not null 
            {
                throw new FriendAlreadyExistException("Friendship already exist."); 
            }

            // if the user is UserA then UserB is the friend, else vice versa
            var friendUser = createdFriend.UserAId == createDTO.UserAId ? createdFriend.UserB : createdFriend.UserA;

            var createdFriendDTO = new FriendReadDTO
            {
                FriendId = friendUser.Id,
                Firstname = friendUser.Firstname,
                LastName = friendUser.Lastname
            };
            return createdFriendDTO;
        }

        public async Task<FriendReadDTO> DeleteFriendAsync(int userId, int friendId)
        {
            var aId = Math.Min(userId, friendId);
            var bId = Math.Max(userId, friendId);
;
            var deletedFriend = await _friendRepository.DeleteFriendAsync(aId, bId);
            if (deletedFriend is null) 
            {
                throw new FriendNotFoundException("Friend not found.");
            }

            // if the userA is userAid, then UserB is the friends, else vise versa
            var friendUser = deletedFriend.UserAId == userId ? deletedFriend.UserB : deletedFriend.UserA;

            var deletedFriendDTO = new FriendReadDTO
            {
                FriendId = friendUser.Id,
                Firstname = friendUser.Firstname,
                LastName = friendUser.Lastname
            };

            return deletedFriendDTO;
        }

        public async Task<List<FriendReadDTO>> GetAllFriendsAsync(int userId)
        {
            var friends = await _friendRepository.GetAllFriendsAsync(userId);

            var friendDTOs = friends.Select(f => 
            {
                var friendUser = f.UserAId == userId ? f.UserB : f.UserA;

                return new FriendReadDTO
                {
                    FriendId = friendUser.Id,
                    Firstname = friendUser.Firstname,
                    LastName = friendUser.Lastname
                };
            }).ToList();

            return friendDTOs;
        }

        public async Task<FriendReadDTO> GetFriendAsync(int userId, int friendId)
        {
            var aId = Math.Min(userId, friendId);
            var bId = Math.Max(userId, friendId);

            var friend = await _friendRepository.GetFriendAsync(aId, bId);
            if (friend is null) 
            {
                Console.WriteLine(aId + " " + bId);
                throw new FriendNotFoundException("Friend not found");
            }

            var friendUser = friend.UserAId == userId ? friend.UserB : friend.UserA;

            var friendDTO = new FriendReadDTO
            {
                FriendId = friendUser.Id,
                Firstname = friendUser.Firstname,
                LastName = friendUser.Lastname
            };

            return friendDTO;
        }
    }
}
