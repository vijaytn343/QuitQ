using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.AuthDTOs;
using QuitQ.Models;
using QuitQ.Services.AuthFeature;
using QuitQ.Services.EmailFeature;

namespace QuitQ.Test.Tests
{
    public class AuthServiceTests
    {
        private AppDbContext _context;
        private AuthService _service;

        [SetUp]
        public void Setup()
        {
            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var loggerMock =
                new Mock<ILogger<AuthService>>();

            var emailMock =
                new Mock<IEmailService>();

            var configuration =
                new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        { "Jwt:Key", "ThisIsASuperSecretKeyForTesting12345" },
                        { "Jwt:Issuer", "TestIssuer" },
                        { "Jwt:Audience", "TestAudience" }
                    })
                .Build();

            _service = new AuthService(
                _context,
                configuration,
                loggerMock.Object,
                emailMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task Register_ShouldCreateUser_WhenValidData()
        {
            // Arrange
            _context.Roles.Add(new Role
            {
                RoleId = 1,
                RoleName = "Customer"
            });

            await _context.SaveChangesAsync();

            var dto = new RegisterDTO
            {
                Name = "Vijay",
                Email = "vijay@test.com",
                Password = "Password123",
                Phone = "9876543210",
                Role = "Customer"
            };

            // Act
            var result = await _service.RegisterAsync(dto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(dto.Email));
        }

        [Test]
        public void Register_ShouldThrow_WhenEmailExists()
        {
            // Arrange
            _context.Roles.Add(new Role
            {
                RoleId = 1,
                RoleName = "Customer"
            });

            _context.Users.Add(new User
            {
                Name = "Vijay",
                Email = "vijay@test.com",
                PasswordHash = "hash",
                Phone = "9876543210",
                RoleId = 1,
                IsActive = true
            });

            _context.SaveChanges();

            var dto = new RegisterDTO
            {
                Name = "New User",
                Email = "vijay@test.com",
                Password = "Password123",
                Phone = "9999999999",
                Role = "Customer"
            };

            // Act + Assert
            Assert.ThrowsAsync<Exception>(
                async () => await _service.RegisterAsync(dto));
        }

        [Test]
        public async Task Login_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange
            var role = new Role
            {
                RoleId = 1,
                RoleName = "Customer"
            };

            _context.Roles.Add(role);

            _context.Users.Add(new User
            {
                UserId = 1,
                Name = "Vijay",
                Email = "vijay@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Phone = "9876543210",
                RoleId = 1,
                Role = role,
                IsActive = true
            });

            await _context.SaveChangesAsync();

            var dto = new LoginDTO
            {
                Email = "vijay@test.com",
                Password = "Password123"
            };

            // Act
            var result = await _service.LoginAsync(dto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Login_ShouldThrow_WhenPasswordInvalid()
        {
            // Arrange
            var role = new Role
            {
                RoleId = 1,
                RoleName = "Customer"
            };

            _context.Roles.Add(role);

            _context.Users.Add(new User
            {
                UserId = 1,
                Name = "Vijay",
                Email = "vijay@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Phone = "9876543210",
                RoleId = 1,
                Role = role,
                IsActive = true
            });

            _context.SaveChanges();

            var dto = new LoginDTO
            {
                Email = "vijay@test.com",
                Password = "WrongPassword"
            };

            // Act + Assert
            Assert.ThrowsAsync<Exception>(
                async () => await _service.LoginAsync(dto));
        }
    }
}