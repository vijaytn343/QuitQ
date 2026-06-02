using QuitQ.Data;
using QuitQ.DTOs.CategoryDTOs;
using QuitQ.Models;
using Microsoft.EntityFrameworkCore;
using QuitQ.Services.Interfaces;

namespace QuitQ.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryResponseDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .ToListAsync();
        }

        public async Task<CategoryResponseDTO?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryResponseDTO
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    Description = c.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CategoryResponseDTO> CreateCategoryAsync(CategoryCreateDTO dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryResponseDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
