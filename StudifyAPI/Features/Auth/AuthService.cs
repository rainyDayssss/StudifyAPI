using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudifyAPI.Features.Auth.DTOs;
using StudifyAPI.Features.Auth.Model;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Features.UserStreaks.Model;
using StudifyAPI.Shared.Database;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Features.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly StudifyDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IMapper mapper,
            StudifyDbContext context,
            IConfiguration config)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<User>();
            _mapper = mapper;
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDTO> RegisterAsync(UserCreateDTO userCreateDTO)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userCreateDTO.Email);

            if (existingUser is not null)
            {
                throw new EmailAlreadyUsedException("Email is already used");
            }

            var user = _mapper.Map<User>(userCreateDTO);
            user.IsOnline = true;
            user.Streak = new UserStreak()
            {
                CurrentStreakDays = 0,
                LastUpdated = DateTime.UtcNow
            };

            user.Password = _passwordHasher.HashPassword(user, userCreateDTO.Password);

            User createdUser = await _userRepository.CreateUserAsync(user);

            // Auto-login: issue tokens immediately after registration
            return await GenerateAuthResponseAsync(createdUser);
        }

        public async Task<AuthResponseDTO> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(userLoginDTO.Email);
            if (existingUser is null)
            {
                throw new UserNotFoundException("User not found");
            }

            var result = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, userLoginDTO.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidPasswordException("Invalid password");
            }

            await _userRepository.LoginAsync(existingUser.Id);

            return await GenerateAuthResponseAsync(existingUser);
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            // Look up the token in the database
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");
            }

            // Revoke the old token (Refresh Token Rotation)
            storedToken.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Issue brand new access + refresh token pair
            return await GenerateAuthResponseAsync(storedToken.User);
        }

        public async Task<UserReadDTO> LogoutAsync(int userId)
        {
            var existingUser = await _userRepository.LogoutAsync(userId);
            if (existingUser is null)
            {
                throw new UserNotFoundException("User not found");
            }

            // Revoke ALL active refresh tokens for this user
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<UserReadDTO>(existingUser);
        }

        // ----------------------
        // Private Helpers
        // ----------------------
        private async Task<AuthResponseDTO> GenerateAuthResponseAsync(User user)
        {
            var refreshTokenExpiryInDays = Convert.ToDouble(
                _config["JwtSettings:RefreshTokenExpiryInDays"] ?? "7");
            var expiryInMinutes = Convert.ToDouble(
                _config["JwtSettings:ExpiryInMinutes"] ?? "15");

            var accessToken = _jwtService.GenerateAccessToken(user.Email, user.Id);
            var refreshTokenValue = _jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpiryInDays),
                CreatedAt = DateTime.UtcNow
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(expiryInMinutes)
            };
        }
    }
}
