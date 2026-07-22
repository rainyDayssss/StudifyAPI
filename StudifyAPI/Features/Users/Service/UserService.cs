using AutoMapper;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserReadDTO> DeleteUserAsync(int id)
        {
            var deletedUser = await _userRepository.DeleteUserAsync(id);
            if (deletedUser is null) {
                throw new UserNotFoundException("User not found");
            }
            return _mapper.Map<UserReadDTO>(deletedUser);
        }

        public async Task<List<UserReadDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<List<UserReadDTO>>(users);
        }

        public async Task<UserReadDTO> GetUserByEmailAsync(string email)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser is null) 
            {
                throw new UserNotFoundException("User not found");
            }
            return _mapper.Map<UserReadDTO>(existingUser);
        }

        public async Task<UserReadDTO> GetUserByIdAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser is null) {
                throw new UserNotFoundException("User not found");
            }
            return _mapper.Map<UserReadDTO>(existingUser);
        }

        public async Task<UserReadDTO> PatchUserAsync(int id, UserPatchDTO userPatchDTO)
        {
            var existingUser = await _userRepository.PatchUserAsync(id, userPatchDTO);
            if (existingUser is null) { 
                throw new UserNotFoundException("User not found");
            }
            return _mapper.Map<UserReadDTO>(existingUser);
        }
    }
}
