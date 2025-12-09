using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.Friends.Model;
using StudifyAPI.Shared.Database;

namespace StudifyAPI.Features.Friends.Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly StudifyDbContext _context;
        public FriendRepository(StudifyDbContext context) {
            _context = context;
        }
        public async Task<Friend?> AddFriendAsync(Friend friend)
        {
            await _context.Friends.AddAsync(friend);
            await _context.SaveChangesAsync();
            await _context.Entry(friend).Reference(fr => fr.UserA).LoadAsync();
            await _context.Entry(friend).Reference(fr => fr.UserB).LoadAsync();
            return friend;
        }


        public async Task DecrementFriendCountAsync(Friend friend)
        {
            if (friend.UserA != null) friend.UserA.NumberOfFriends--;
            if (friend.UserB != null) friend.UserB.NumberOfFriends--;

            await _context.SaveChangesAsync();
        }


        public async Task<Friend?> DeleteFriendAsync(int userId, int friendId)
        {
            var friend = await _context.Friends
                .Include(f => f.UserA)
                .Include(f => f.UserB)
                .FirstOrDefaultAsync(f =>
                    (f.UserAId == userId && f.UserBId == friendId) ||
                    (f.UserAId == friendId && f.UserBId == userId));

            if (friend is null) {
                return null;
            }

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
            return friend;
        }

        public async Task<List<Friend>> GetAllFriendsAsync(int userId)
        {
            var friends = await _context.Friends
                .Include(f => f.UserA)
                .Include(f => f.UserB)
                .Where(f => f.UserAId == userId || f.UserBId == userId)
                .ToListAsync();
            return friends;
        }

        public async Task<Friend?> GetFriendAsync(int userId, int friendId)
        {
            var friend = await _context.Friends
                .Include(f => f.UserA)
                .Include(f => f.UserB)
                .FirstOrDefaultAsync(f =>
                    (f.UserAId == userId && f.UserBId == friendId) ||
                    (f.UserAId == friendId && f.UserBId == userId));

            return friend;
        }
    }
}
