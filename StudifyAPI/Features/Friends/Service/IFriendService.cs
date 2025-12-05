using StudifyAPI.Features.Friends.DTO;

namespace StudifyAPI.Features.Friends.Service
{
    public interface IFriendService
    {
        public Task<FriendReadDTO> AddFriendAsync(FriendCreateDTO createDTO);
        public Task<List<FriendReadDTO>> GetAllFriendsAsync(int userId);
        public Task<FriendReadDTO> GetFriendAsync(int userId, int friendId);
        public Task<FriendReadDTO> DeleteFriendAsync(int userId, int friendId);
    }
}
