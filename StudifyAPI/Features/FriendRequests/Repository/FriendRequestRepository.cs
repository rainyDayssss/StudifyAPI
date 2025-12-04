using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StudifyAPI.Features.FriendRequests.Model;
using StudifyAPI.Shared.Database;

namespace StudifyAPI.Features.FriendRequests.Repository
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly StudifyDbContext _context;
        public FriendRequestRepository(StudifyDbContext context)
        {
            _context = context;
        }
        public Task<FriendRequest?> AcceptFriendRequestAsync(int senderId, int receiverId)
        {
            // delete the friend request and create a friend relation
            // this will talk to the friends repository to create a friend relation
            throw new NotImplementedException(); 
        }

        public async Task<FriendRequest> CreateFriendRequestAsysnc(FriendRequest friendRequest)
        {
            await _context.FriendRequests.AddAsync(friendRequest);
            await _context.SaveChangesAsync();
            return friendRequest;
        }

        // cancel sent request
        public async Task<FriendRequest?> DeleteFriendRequestAsync(int senderId, int receiverId)
        {
            var sentRequest = _context.FriendRequests
                .FirstOrDefault(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);
            if (sentRequest is null) 
            {
                return null;
            }
            _context.FriendRequests.Remove(sentRequest);
            await _context.SaveChangesAsync();
            return sentRequest;
        }

        public async Task<List<FriendRequest>> GetAllReceivedRequestsAsync(int userId)
        {
            var receivedRequests = await _context.FriendRequests
                .Where(fr => fr.ReceiverId == userId)
                .ToListAsync();
            return receivedRequests;
        }

        public async Task<List<FriendRequest>> GetAllSentRequestsAsync(int userId)
        {
            var sentRequests = await _context.FriendRequests
                .Where(fr => fr.SenderId == userId)
                .ToListAsync();
            return sentRequests;
        }
    }
}
