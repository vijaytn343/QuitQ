using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.PaymentDTOs;
using QuitQ.Mappings;
using QuitQ.Models;
using QuitQ.Services.EmailFeature;
using QuitQ.Services.PaymentFeature;


namespace QuitQ.Test.Tests
{
    public class PaymentServiceTests
    {
        private AppDbContext _context;
        private PaymentService _service;

        [SetUp]
        public void Setup()
        {
            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var mapperConfig =
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();
                });

            var mapper = mapperConfig.CreateMapper();

            var loggerMock =
                new Mock<ILogger<PaymentService>>();

            var emailMock =
                new Mock<IEmailService>();

            _service = new PaymentService(
                _context,
                mapper,
                loggerMock.Object,
                emailMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetPaymentById_ShouldReturnPayment_WhenExists()
        {
            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                OrderStatus = "Pending",
                TotalAmount = 1000
            };

            _context.Orders.Add(order);

            var payment = new Payment
            {
                PaymentId = 1,
                OrderId = 1,
                PaymentMethod = "UPI",
                Amount = 1000,
                PaymentStatus = "Pending"
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            var result =
                await _service.GetPaymentByIdAsync(
                    1,
                    1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.PaymentId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetPaymentById_ShouldReturnNull_WhenNotFound()
        {
            var result =
                await _service.GetPaymentByIdAsync(
                    999,
                    1);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdatePaymentStatus_ShouldReturnTrue_WhenPaymentExists()
        {
            var payment = new Payment
            {
                PaymentId = 1,
                OrderId = 1,
                PaymentMethod = "UPI",
                Amount = 1000,
                PaymentStatus = "Pending"
            };

            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();

            var result =
                await _service.UpdatePaymentStatusAsync(
                    1,
                    "Success",
                    "TXN123");

            Assert.That(result, Is.True);

            var updatedPayment =
                await _context.Payments.FindAsync(1);

            Assert.That(
                updatedPayment!.PaymentStatus,
                Is.EqualTo("Success"));
        }

        [Test]
        public async Task UpdatePaymentStatus_ShouldReturnFalse_WhenPaymentNotFound()
        {
            var result =
                await _service.UpdatePaymentStatusAsync(
                    999,
                    "Success",
                    "TXN123");

            Assert.That(result, Is.False);
        }
    }
}