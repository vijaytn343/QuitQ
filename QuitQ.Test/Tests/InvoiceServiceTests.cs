using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.Models;
using QuestPDF.Infrastructure;
using QuitQ.Services.InvoiceFeature;

namespace QuitQ.Test.Tests
{
    public class InvoiceServiceTests
    {
        private AppDbContext _context;
        private InvoiceService _service;

        [SetUp]
        public void Setup()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _service = new InvoiceService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GenerateInvoice_ShouldThrow_WhenOrderNotFound()
        {
            Assert.Throws<Exception>(() =>
                _service.GenerateInvoice(999, 1));
        }

        [Test]
        public void GenerateInvoice_ShouldThrow_WhenPaymentNotFound()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Vijay",
                Email = "vijay@test.com"
            };

            var address = new Address
            {
                AddressId = 1,
                UserId = 1,
                FullAddress = "Chennai"
            };

            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                AddressId = 1,
                TotalAmount = 1000
            };

            _context.Users.Add(user);
            _context.Addresses.Add(address);
            _context.Orders.Add(order);
            _context.SaveChanges();

            Assert.Throws<Exception>(() =>
                _service.GenerateInvoice(1, 1));
        }

        [Test]
        public void GenerateInvoice_ShouldThrow_WhenPaymentNotPaid()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Vijay",
                Email = "vijay@test.com"
            };

            var address = new Address
            {
                AddressId = 1,
                UserId = 1,
                FullAddress = "Chennai"
            };

            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                AddressId = 1,
                TotalAmount = 1000
            };

            var payment = new Payment
            {
                PaymentId = 1,
                OrderId = 1,
                Amount = 1000,
                PaymentMethod = "UPI",
                PaymentStatus = "Pending"
            };

            _context.Users.Add(user);
            _context.Addresses.Add(address);
            _context.Orders.Add(order);
            _context.Payments.Add(payment);

            _context.SaveChanges();

            Assert.Throws<Exception>(() =>
                _service.GenerateInvoice(1, 1));
        }

        [Test]
        public void GenerateInvoice_ShouldReturnPdf_WhenPaymentPaid()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Vijay",
                Email = "vijay@test.com"
            };

            var address = new Address
            {
                AddressId = 1,
                UserId = 1,
                FullAddress = "Chennai",
                City = "Chennai",
                State = "Tamil Nadu",
                Country = "India",
                Pincode = "600001"
            };

            var order = new Order
            {
                OrderId = 1,
                UserId = 1,
                AddressId = 1,
                TotalAmount = 1000,
                User = user,
                Address = address
            };

            var payment = new Payment
            {
                PaymentId = 1,
                OrderId = 1,
                Amount = 1000,
                PaymentMethod = "UPI",
                PaymentStatus = "Paid",
                TransactionId = "TXN123"
            };

            _context.Users.Add(user);
            _context.Addresses.Add(address);
            _context.Orders.Add(order);
            _context.Payments.Add(payment);

            _context.SaveChanges();

            var result =
                _service.GenerateInvoice(1, 1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));
        }
    }
}