using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.UserDTOs;
using QuitQ.Models;
using QuitQ.Services.UserFeature;

namespace QuitQ.Test.Tests
{
    public class UserServiceTests
    {
        private AppDbContext _context;
        private UserService _service;

        [SetUp]
        public void Setup()
        {
            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var mapperMock =
                new Mock<IMapper>();

            _service = new UserService(
                _context,
                mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetUserById_ShouldReturnNull_WhenUserNotFound()
        {
            var result =
                await _service.GetUserByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateUser_ShouldReturnFalse_WhenUserNotFound()
        {
            var dto = new UserUpdateDTO
            {
                Name = "Updated User",
                Phone = "9999999999",
                Gender = "Male"
            };

            var result =
                await _service.UpdateUserAsync(
                    1,
                    999,
                    dto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnFalse_WhenUserNotFound()
        {
            var result =
                await _service.DeleteUserAsync(999);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnTrue_WhenUserExists()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Vijay",
                Email = "vijay@test.com",
                PasswordHash = "hash",
                RoleId = 1,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result =
                await _service.DeleteUserAsync(1);

            Assert.That(result, Is.True);

            var updatedUser =
                await _context.Users.FindAsync(1);

            Assert.That(updatedUser.IsActive, Is.False);
        }
    }
}