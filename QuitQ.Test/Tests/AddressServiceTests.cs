using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.AddressDTOs;
using QuitQ.Models;
using QuitQ.Services.AddressService;

namespace QuitQ.Test.Tests
{
    public class AddressServiceTests
    {
        private AppDbContext _context;
        private AddressService _service;

        [SetUp]
        public void Setup()
        {
            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<AddressService>>();

            _service = new AddressService(
                _context,
                mapperMock.Object,
                loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAddressById_ShouldReturnNull_WhenAddressNotFound()
        {
            var result = await _service.GetAddressByIdAsync(1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateAddress_ShouldReturnFalse_WhenAddressNotFound()
        {
            var dto = new AddressUpdateDTO
            {
                FullAddress = "New Address",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                Pincode = "600001"
            };

            var result =
                await _service.UpdateAddressAsync(
                    1,
                    999,
                    dto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAddress_ShouldReturnFalse_WhenAddressNotFound()
        {
            var result =
                await _service.DeleteAddressAsync(
                    1,
                    999);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAddress_ShouldReturnTrue_WhenAddressExists()
        {
            var address = new Address
            {
                AddressId = 1,
                UserId = 1,
                FullAddress = "Test Address",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                Pincode = "600001"
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var result =
                await _service.DeleteAddressAsync(
                    1,
                    1);

            Assert.That(result, Is.True);

            var deletedAddress =
                await _context.Addresses.FindAsync(1);

            Assert.That(deletedAddress, Is.Null);
        }
    }
}