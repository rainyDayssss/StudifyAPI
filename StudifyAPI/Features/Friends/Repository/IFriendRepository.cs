using StudifyAPI.Features.Friends.Model;

namespace StudifyAPI.Features.Friends.Repository
{
    public interface IFriendRepository 
    {
        public Task<Friend?> AddFriendAsync(Friend friend);
        public Task<List<Friend>> GetAllFriendsAsync(int userId);
        public Task<Friend?> GetFriendAsync(int userId, int friendId);
        public Task<Friend?> DeleteFriendAsync(int userId, int friendId); 
        public Task DecrementFriendCountAsync(Friend friend);
    }
}
