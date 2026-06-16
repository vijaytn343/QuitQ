using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.ProductDTOs;
using QuitQ.Models;
using QuitQ.Services.ProductFeature;

namespace QuitQ.Test.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext _context;
        private ProductService _service;

        [SetUp]
        public void Setup()
        {
            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ProductService>>();

            _service = new ProductService(
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
        public async Task GetProductById_ShouldReturnNull_WhenProductNotFound()
        {
            var result =
                await _service.GetProductByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task CreateProduct_ShouldThrow_WhenSellerNotFound()
        {
            var dto = new ProductCreateDTO
            {
                ProductName = "Laptop",
                QuantityAvailable = 10
            };

            Assert.ThrowsAsync<Exception>(
                async () =>
                    await _service.CreateProductAsync(
                        999,
                        dto));
        }

        [Test]
        public async Task UpdateProduct_ShouldReturnFalse_WhenSellerNotFound()
        {
            var dto = new ProductUpdateDTO
            {
                ProductName = "Updated Product",
                QuantityAvailable = 20
            };

            var result =
                await _service.UpdateProductAsync(
                    999,
                    1,
                    dto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnFalse_WhenSellerNotFound()
        {
            var result =
                await _service.DeleteProductAsync(
                    999,
                    1);

            Assert.That(result, Is.False);
        }
    }
}