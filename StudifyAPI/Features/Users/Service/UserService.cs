using StudifyAPI.Features.Auth;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Features.UserStreaks.Model;
using StudifyAPI.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

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
            
            var existingUser = await _userRepository.GetUserByEmailAsync(userCreateDTO.Email);

            // Check is email already is used
            if (existingUser is not null)
            {
                throw new EmailAlreadyUsedException("Email is already used");
            }


            // mapp userCreateDTO to user entity
            User user = new()
            {
                Firstname = userCreateDTO.Firstname,
                Lastname = userCreateDTO.Lastname,
                Email = userCreateDTO.Email,
                Password = userCreateDTO.Password,
                IsOnline = false, // default
                Streak = new UserStreak()
                {
                    CurrentStreakDays = 0, // default 
                    LastUpdated = DateTime.UtcNow
                }
            };

            // Save the user enity 
            User createdUser = await _userRepository.CreateUserAsync(user);

            // map created user entity to UserReadDTO
            var createdUserDTO = new UserReadDTO()
            {
                Id = createdUser.Id,
                Firstname = createdUser.Firstname,
                Lastname = createdUser.Lastname,
                Email = createdUser.Email,
                IsOnline = createdUser.IsOnline,
                CurrentStreakDays = createdUser.Streak.CurrentStreakDays
            };

            return createdUserDTO;
        }

        public async Task<UserReadDTO> DeleteUserAsync(int id)
        {
            var deletedUser = await _userRepository.DeleteUserAsync(id);
            if (deletedUser is null) {
                throw new UserNotFoundException("User not found");
            }
            var deletedUserDTO = new UserReadDTO()
            {
                Id = deletedUser.Id,
                Firstname = deletedUser.Firstname,
                Lastname = deletedUser.Lastname,
                Email = deletedUser.Email,
                IsOnline = deletedUser.IsOnline,
                CurrentStreakDays = deletedUser.Streak.CurrentStreakDays
            };
            return deletedUserDTO;
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
                IsOnline = user.IsOnline,
                CurrentStreakDays = user.Streak.CurrentStreakDays
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
            
            var userReadDTO = new UserReadDTO()
            {
                Id = existingUser.Id,
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                IsOnline = existingUser.IsOnline,
                CurrentStreakDays = existingUser.Streak.CurrentStreakDays
            };

            // return the mapped UserReadDTO
            return userReadDTO;
        }

        public async Task<UserReadDTO> GetUserByIdAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser is null) {
                throw new UserNotFoundException("User not found");
            }
            var userReadDTO = new UserReadDTO()
            {
                Id = existingUser.Id,
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                IsOnline = existingUser.IsOnline,
                NumberOfFriends = existingUser.NumberOfFriends,
                CurrentStreakDays = existingUser.Streak.CurrentStreakDays
            };
            return userReadDTO;
        }

        public async Task<string> LoginAsync(UserLoginDTO userLoginDTO)
        {
            // verify user exists
            var existingUser = await _userRepository.GetUserByEmailAsync(userLoginDTO.Email);
            if (existingUser is null)
            {
                throw new UserNotFoundException("User not found");
            }

            // verify password
            if (existingUser.Password != userLoginDTO.Password)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            // set user IsOnline to true
            await _userRepository.LoginAsync(existingUser.Id);
            // return the mapped UserReadDTO
            return _jwtService.GenerateToken(existingUser.Email, existingUser.Id);
        }

        public async Task<UserReadDTO> LogoutAsync(int userId)
        {
            var existingUser = await _userRepository.LogoutAsync(userId);
            if (existingUser is null) {
                throw new UserNotFoundException("User not found");
            }

            var logoutUserDTO = new UserReadDTO()
            {
                Id = existingUser.Id,
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                IsOnline = existingUser.IsOnline,
                CurrentStreakDays = existingUser.Streak.CurrentStreakDays
            };
            return logoutUserDTO;
        }

        public async Task<UserReadDTO> PatchUserAsync(int id, UserPatchDTO userPatchDTO)
        {
            var existingUser = await _userRepository.PatchUserAsync(id, userPatchDTO);
            if (existingUser is null) { 
                throw new UserNotFoundException("User not found");
            }
            var updatedUserDTO = new UserReadDTO()
            {
                Id = existingUser.Id,
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                IsOnline = existingUser.IsOnline,
                CurrentStreakDays = existingUser.Streak.CurrentStreakDays
            };
            return updatedUserDTO;
        }
    }
}
