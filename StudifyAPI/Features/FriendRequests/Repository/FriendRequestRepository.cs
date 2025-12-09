using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StudifyAPI.Features.FriendRequests.Model;
using StudifyAPI.Features.Users.Models;
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
        

        public async Task<FriendRequest> CreateFriendRequestAsync(FriendRequest friendRequest)
        {
            await _context.FriendRequests.AddAsync(friendRequest);
            await _context.SaveChangesAsync();
            await _context.Entry(friendRequest).Reference(fr => fr.Sender).LoadAsync();
            await _context.Entry(friendRequest).Reference(fr => fr.Receiver).LoadAsync();
            return friendRequest;
        }

        // cancel sent request
        public async Task<FriendRequest?> CancelFriendRequestAsync(int requestId, int userId)
        {
            var sentRequest = await _context.FriendRequests
                .Include(fr => fr.Sender)
                .Include(fr => fr.Receiver)
                .FirstOrDefaultAsync(fr => fr.Id == requestId && fr.SenderId == userId);
            if (sentRequest is null)
            {
                return null;
            }
            _context.FriendRequests.Remove(sentRequest);
            await _context.SaveChangesAsync();
            return sentRequest;
        }

        // reject received request
        public async Task<FriendRequest?> RejectFriendRequestAsync(int requestId, int userId) {
            var sentRequest = await _context.FriendRequests
               .Include(fr => fr.Sender)
               .Include(fr => fr.Receiver)
               .FirstOrDefaultAsync(fr => fr.Id == requestId && fr.ReceiverId == userId);
            if (sentRequest is null)
            {
                return null;
            }
            _context.FriendRequests.Remove(sentRequest);
            await _context.SaveChangesAsync();
            return sentRequest;
        }


        public async Task<List<FriendRequest>> GetAllReceivedRequestsAsync(int receiverId)
        {
            var receivedRequests = await _context.FriendRequests
                .Where(fr => fr.ReceiverId == receiverId)
                .Include(fr => fr.Sender) // get all the sender of the requests
                .Include(fr => fr.Receiver)
                .ToListAsync();
            return receivedRequests;
        }

        public async Task<List<FriendRequest>> GetAllSentRequestsAsync(int senderId)
        {
            var sentRequests = await _context.FriendRequests
                .Where(fr => fr.SenderId == senderId)
                .Include(fr => fr.Sender) 
                .Include(fr => fr.Receiver) // get all the receiver of the requests
                .ToListAsync();
            return sentRequests;
        }

        public Task<FriendRequest?> GetFriendRequestBetweenUsersAsync(int senderId, int receiverId)
        {
            var friendRequest = _context.FriendRequests
                .FirstOrDefaultAsync(fr => 
                    (fr.SenderId == senderId && fr.ReceiverId == receiverId) ||
                    (fr.ReceiverId == senderId && fr.SenderId == receiverId)            
                );
                
            return friendRequest;
        }

        public async Task<FriendRequest?> GetFriendRequestAsync(int requestId)
        {
            return await _context.FriendRequests
                .Include(fr => fr.Sender)
                .Include(fr => fr.Receiver)
                .FirstOrDefaultAsync(fr => fr.Id == requestId);
        }

        public async Task<FriendRequest?> DeleteFriendRequestAsync(FriendRequest friendRequest)
        {
            _context.FriendRequests.Remove(friendRequest);
            await _context.SaveChangesAsync();
            return friendRequest;
        }
    }
}
