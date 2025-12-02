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
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return null;
            }
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
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
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
