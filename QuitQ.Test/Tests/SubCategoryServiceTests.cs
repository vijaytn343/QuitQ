using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using QuitQ.Data;
using QuitQ.DTOs.SubCategoryDTO;
using QuitQ.Models;
using QuitQ.Services.SubCategoryFeature;

namespace QuitQ.Test.Tests
{
    public class SubCategoryServiceTests
    {
        private AppDbContext _context;
        private SubCategoryService _service;

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

            _service = new SubCategoryService(
                _context,
                mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetSubCategoryById_ShouldReturnNull_WhenSubCategoryNotFound()
        {
            var result =
                await _service.GetSubCategoryByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateSubCategory_ShouldReturnFalse_WhenSubCategoryNotFound()
        {
            var dto = new SubCategoryUpdateDTO
            {
                SubCategoryName = "Updated SubCategory"
            };

            var result =
                await _service.UpdateSubCategoryAsync(
                    999,
                    dto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteSubCategory_ShouldReturnFalse_WhenSubCategoryNotFound()
        {
            var result =
                await _service.DeleteSubCategoryAsync(999);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteSubCategory_ShouldReturnTrue_WhenSubCategoryExists()
        {
            var category = new Category
            {
                CategoryId = 1,
                CategoryName = "Electronics"
            };

            var subCategory = new SubCategory
            {
                SubCategoryId = 1,
                SubCategoryName = "Mobiles",
                CategoryId = 1
            };

            _context.Categories.Add(category);
            _context.SubCategories.Add(subCategory);

            await _context.SaveChangesAsync();

            var result =
                await _service.DeleteSubCategoryAsync(1);

            Assert.That(result, Is.True);

            var deletedSubCategory =
                await _context.SubCategories.FindAsync(1);

            Assert.That(deletedSubCategory, Is.Null);
        }
    }
}