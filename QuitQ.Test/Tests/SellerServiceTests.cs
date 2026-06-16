using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.SellerDTOs;
using QuitQ.Models;
using QuitQ.Services.SellerFeature;

namespace QuitQ.Test.Tests
{
    public class SellerServiceTests
    {
        private AppDbContext _context;
        private SellerService _service;

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

            _service = new SellerService(
                _context,
                mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetSellerById_ShouldReturnNull_WhenSellerNotFound()
        {
            var result =
                await _service.GetSellerByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateSeller_ShouldThrow_WhenSellerAlreadyExists()
        {
            _context.Sellers.Add(new Seller
            {
                SellerId = 1,
                UserId = 1,
                StoreName = "Test Store"
            });

            await _context.SaveChangesAsync();

            var dto = new SellerCreateDTO
            {
                StoreName = "New Store"
            };

            Assert.ThrowsAsync<Exception>(
                async () =>
                    await _service.CreateSellerAsync(
                        1,
                        dto));
        }

        [Test]
        public async Task UpdateSeller_ShouldReturnFalse_WhenSellerNotFound()
        {
            var dto = new SellerUpdateDTO
            {
                StoreName = "Updated Store"
            };

            var result =
                await _service.UpdateSellerByUserIdAsync(
                    999,
                    dto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteSeller_ShouldReturnFalse_WhenSellerNotFound()
        {
            var result =
                await _service.DeleteSellerAsync(999);

            Assert.That(result, Is.False);
        }
    }
}