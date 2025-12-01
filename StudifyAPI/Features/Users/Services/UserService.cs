using Microsoft.Identity.Client;
using StudifyAPI.Common.Exceptions;
using StudifyAPI.Features.Auth;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Features.Users.Repositories;

namespace StudifyAPI.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        public UserService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<UserReadDTO> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            // mapp userCreateDTO to user entity
            User user = new()
            {
                Firstname = userCreateDTO.Firstname,
                Lastname = userCreateDTO.Lastname,
                Email = userCreateDTO.Email,
                Password = userCreateDTO.Password,
                Level = new UserLevel
                {
                    Level = 1, // default level
                    Experience = 0 // default experience
                },
                Streak = new UserStreak
                {
                    CurrentStreak = 0 // default streak
                }
            };

            // Save the user enity 
            User createdUser = await _userRepository.CreateUserAsync(user);

            // map created user entity to UserReadDTO
            return new UserReadDTO()
            {
                Id = createdUser.Id,
                Firstname = createdUser.Firstname,
                Lastname = createdUser.Lastname,
                Email = createdUser.Email,
                Level = createdUser.Level.Level,
                Experience = createdUser.Level.Experience,
                CurrentStreak = createdUser.Streak.CurrentStreak
            };

        }

        public async Task<UserReadDTO> DeleteUserAsync(int id)
        {
            var deletedUser = await _userRepository.DeleteUserAsync(id);
            if (deletedUser is null) {
                throw new UserNotFoundException("User not found");
            }
            return new UserReadDTO()
            {
                Id = deletedUser.Id,
                Firstname = deletedUser.Firstname,
                Lastname = deletedUser.Lastname,
                Email = deletedUser.Email,
                Level = deletedUser.Level.Level,
                Experience = deletedUser.Level.Experience,
                CurrentStreak = deletedUser.Streak.CurrentStreak
            };
        }

        public async Task<List<UserReadDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            // map each user entity to UserReadDTO
            var userReadDTOs = users.Select(user => new UserReadDTO()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Level = user.Level.Level,
                Experience = user.Level.Experience,
                CurrentStreak = user.Streak.CurrentStreak
            }).ToList();
            return userReadDTOs;
        }

        public async Task<UserReadDTO> GetUserByEmailAsync(string email)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser is null) 
            {
                throw new Exception("User not found"); // create more custom exceptions
            }
            // return the mapped UserReadDTO
            return new UserReadDTO()
            {
                Id = existingUser.Id,
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                Level = existingUser.Level.Level,
                Experience = existingUser.Level.Experience,
                CurrentStreak = existingUser.Streak.CurrentStreak
            };
        }

        public async Task<UserReadDTO> GetUserByIdAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser is null) {
                throw new UserNotFoundException("User not found");
            }
            return new UserReadDTO()
            {
                Id = existingUser.Id,
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                Level = existingUser.Level.Level,
                Experience = existingUser.Level.Experience,
                CurrentStreak = existingUser.Streak.CurrentStreak
            };
        }

        public async Task<string> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userLoginDTO.Email); 
            if (existingUser is null)
            {
                throw new UserNotFoundException("User not found");
            }

            // Check is email already is used
            if (userLoginDTO.Email.Equals(existingUser.Email)) {
                throw new EmailAlreadyUsedException("Email is already used");
            }

            // verify password
            if (existingUser.Password != userLoginDTO.Password)
            {
                throw new InvalidPasswordException("Invalid password");
            }


            // return the mapped UserReadDTO
            return _jwtService.GenerateToken(existingUser.Email, existingUser.Id);
        }

        public async Task<UserReadDTO> PatchUserAsync(int id, UserPatchDTO userPatchDTO)
        {
            var existingUser = await _userRepository.PatchUserAsync(id, userPatchDTO);
            if (existingUser is null) { 
                throw new UserNotFoundException("User not found");
            }
            return new UserReadDTO()
            {
                Id = existingUser.Id,
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                Level = existingUser.Level.Level,
                Experience = existingUser.Level.Experience,
                CurrentStreak = existingUser.Streak.CurrentStreak
            };
        }
    }
}
