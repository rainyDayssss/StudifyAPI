using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using StudifyAPI.Features.Auth;
using StudifyAPI.Features.Auth.DTOs;
using StudifyAPI.Features.Users.DTOs;
using StudifyAPI.Features.Users.Models;
using StudifyAPI.Features.Users.Repositories;
using StudifyAPI.Shared.Database;
using StudifyAPI.Shared.Exceptions;

namespace StudifyAPI.Tests.Auth
{
    public class AuthServiceTests : IDisposable
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly StudifyDbContext _dbContext;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();

            // In-memory DB (unique name per test class instance to avoid state leakage)
            var options = new DbContextOptionsBuilder<StudifyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new StudifyDbContext(options);

            // Minimal config for JWT settings
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["JwtSettings:ExpiryInMinutes"] = "15",
                    ["JwtSettings:RefreshTokenExpiryInDays"] = "7"
                })
                .Build();

            // Mapper: UserCreateDTO -> User
            _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserCreateDTO>()))
                .Returns((UserCreateDTO source) => new User
                {
                    Firstname = source.Firstname,
                    Lastname = source.Lastname,
                    Email = source.Email
                });

            // Mapper: User -> UserReadDTO
            _mapperMock.Setup(m => m.Map<UserReadDTO>(It.IsAny<User>()))
                .Returns((User source) => new UserReadDTO
                {
                    Id = source.Id,
                    Firstname = source.Firstname,
                    Lastname = source.Lastname,
                    Email = source.Email,
                    IsOnline = source.IsOnline
                });

            _authService = new AuthService(
                _userRepositoryMock.Object,
                _jwtServiceMock.Object,
                _mapperMock.Object,
                _dbContext,
                _config);
        }

        public void Dispose() => _dbContext.Dispose();

        // -------------------------------------------------------
        // RegisterAsync
        // -------------------------------------------------------

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ThrowsEmailAlreadyUsedException()
        {
            // Arrange
            var dto = new UserCreateDTO { Email = "test@test.com", Password = "Password123" };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync(new User { Email = dto.Email });

            // Act
            Func<Task> act = async () => await _authService.RegisterAsync(dto);

            // Assert
            await act.Should().ThrowAsync<EmailAlreadyUsedException>()
                .WithMessage("Email is already used");

            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_HashesPasswordAndReturnsAuthResponse()
        {
            // Arrange
            var dto = new UserCreateDTO
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "new@test.com",
                Password = "Password123"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync((User?)null);

            _userRepositoryMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => { user.Id = 1; return user; });

            _jwtServiceMock.Setup(jwt => jwt.GenerateAccessToken(dto.Email, 1))
                .Returns("access-token-value");
            _jwtServiceMock.Setup(jwt => jwt.GenerateRefreshToken())
                .Returns("refresh-token-value");

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("access-token-value");
            result.RefreshToken.Should().Be("refresh-token-value");
            result.AccessTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(15), TimeSpan.FromSeconds(5));

            // Verify password was hashed (not stored as plaintext)
            _userRepositoryMock.Verify(repo => repo.CreateUserAsync(
                It.Is<User>(u => u.Password != "Password123" && !string.IsNullOrEmpty(u.Password))),
                Times.Once);

            // Verify a RefreshToken was persisted to the DB
            _dbContext.RefreshTokens.Count().Should().Be(1);
        }

        // -------------------------------------------------------
        // LoginAsync
        // -------------------------------------------------------

        [Fact]
        public async Task LoginAsync_WithWrongPassword_ThrowsInvalidPasswordException()
        {
            // Arrange
            var dto = new UserLoginDTO { Email = "test@test.com", Password = "WrongPassword" };

            var existingUser = new User { Id = 1, Email = "test@test.com" };
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
            existingUser.Password = passwordHasher.HashPassword(existingUser, "CorrectPassword");

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync(existingUser);

            // Act
            Func<Task> act = async () => await _authService.LoginAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidPasswordException>()
                .WithMessage("Invalid password");

            // No token should have been generated
            _jwtServiceMock.Verify(jwt => jwt.GenerateAccessToken(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsAuthResponse()
        {
            // Arrange
            var dto = new UserLoginDTO { Email = "test@test.com", Password = "CorrectPassword" };

            var existingUser = new User { Id = 1, Email = "test@test.com" };
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
            existingUser.Password = passwordHasher.HashPassword(existingUser, "CorrectPassword");

            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.LoginAsync(existingUser.Id))
                .ReturnsAsync(existingUser);

            _jwtServiceMock.Setup(jwt => jwt.GenerateAccessToken(dto.Email, existingUser.Id))
                .Returns("login-access-token");
            _jwtServiceMock.Setup(jwt => jwt.GenerateRefreshToken())
                .Returns("login-refresh-token");

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("login-access-token");
            result.RefreshToken.Should().Be("login-refresh-token");

            // Verify a RefreshToken was persisted to the DB
            _dbContext.RefreshTokens.Count().Should().Be(1);
        }
    }
}
