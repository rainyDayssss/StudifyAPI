using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Shared.Database;

namespace StudifyAPI.Features.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StudifyDbContext _context;
        public UserRepository(StudifyDbContext context)
        {
            _context = context;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);  
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteUserAsync(int id)
        {
            var existingUser = await _context.Users
                .Include(u => u.Streak)
                .Include(u => u.Tasks)
                .Include(u => u.SentFriendRequests)
                .Include(u => u.ReceivedFriendRequests)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
                return null;

            // Remove friend requests first because of Restrict
            _context.FriendRequests.RemoveRange(existingUser.SentFriendRequests);
            _context.FriendRequests.RemoveRange(existingUser.ReceivedFriendRequests);

            // Tasks and Streak will cascade automatically
            _context.Users.Remove(existingUser);

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Streak)
                .ToListAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Streak)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Streak)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> LoginAsync(int userId)
        {
            var existingUser = await _context.Users
                .Include(u => u.Streak)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (existingUser is null) {
                return null;
            }
            existingUser.IsOnline = true; // set IsOnline to true
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<User?> LogoutAsync(int userId)
        {
            var existingUser = await _context.Users
                .Include(u => u.Streak)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.IsOnline = false; // set IsOnline to false
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<User?> PatchUserAsync(int id, UserPatchDTO userPatchDTO)
        {
            var existingUser = await _context.Users
                .Include(u => u.Streak)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
            {
                return null;
            }
            existingUser.Firstname = userPatchDTO.Firstname ?? existingUser.Firstname;
            existingUser.Lastname = userPatchDTO.Lastname ?? existingUser.Lastname;
            existingUser.Password = userPatchDTO.Password ?? existingUser.Password;
            await _context.SaveChangesAsync();
            return existingUser;
        }

        
    }
}
