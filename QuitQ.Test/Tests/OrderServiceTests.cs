using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.Models;
using QuitQ.Services.EmailFeature;
using QuitQ.Services.OrderFeature;

namespace QuitQ.Test.Tests
{
    public class OrderServiceTests
    {
        private AppDbContext _context;
        private OrderService _service;

        [SetUp]
        public void Setup()
        {
            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(
                    databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var mapperMock =
                new Mock<IMapper>();

            var loggerMock =
                new Mock<ILogger<OrderService>>();

            var emailMock =
                new Mock<IEmailService>();

            _service = new OrderService(
                _context,
                mapperMock.Object,
                loggerMock.Object,
                emailMock.Object);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task UpdateOrderStatus_ShouldReturnTrue_WhenOrderExists()
        {
            // Arrange
            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                OrderStatus = "Pending",
                OrderDate = DateTime.Now,
                TotalAmount = 1000
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result =
                await _service.UpdateOrderStatusAsync(
                    1,
                    "Shipped");

            // Assert
            Assert.That(result, Is.True);

            var updatedOrder =
                await _context.Orders.FindAsync(1);

            Assert.That(
                updatedOrder.OrderStatus,
                Is.EqualTo("Shipped"));
        }
        [Test]
        public async Task UpdateOrderStatus_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            // Act
            var result =
                await _service.UpdateOrderStatusAsync(
                    999,
                    "Shipped");

            // Assert
            Assert.That(result, Is.False);
        }
    }
}