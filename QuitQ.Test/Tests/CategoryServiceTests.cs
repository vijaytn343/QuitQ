using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.CategoryDTOs;
using QuitQ.Models;
using QuitQ.Services.CategoryFeature;

namespace QuitQ.Test.Tests
{
    public class CategoryServiceTests
    {
        private AppDbContext _context;
        private CategoryService _service;

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

            _service = new CategoryService(
                _context,
                mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetCategoryById_ShouldReturnNull_WhenCategoryNotFound()
        {
            var result =
                await _service.GetCategoryByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateCategory_ShouldReturnFalse_WhenCategoryNotFound()
        {
            var dto = new CategoryUpdateDTO
            {
                CategoryName = "Updated Category"
            };

            var result =
                await _service.UpdateCategoryAsync(
                    999,
                    dto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteCategory_ShouldReturnFalse_WhenCategoryNotFound()
        {
            var result =
                await _service.DeleteCategoryAsync(999);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteCategory_ShouldReturnTrue_WhenCategoryExists()
        {
            var category = new Category
            {
                CategoryId = 1,
                CategoryName = "Electronics"
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            var result =
                await _service.DeleteCategoryAsync(1);

            Assert.That(result, Is.True);

            var deletedCategory =
                await _context.Categories.FindAsync(1);

            Assert.That(deletedCategory, Is.Null);
        }
    }
}