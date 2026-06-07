using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.SubCategoryDTO;
using QuitQ.Models;

namespace QuitQ.Services.SubCategoryFeature
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly AppDbContext _context;

        public SubCategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SubCategoryResponseDTO>> GetAllSubCategoriesAsync()
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .Select(sc => new SubCategoryResponseDTO
                {
                    SubCategoryId = sc.SubCategoryId,
                    CategoryId = sc.CategoryId,
                    CategoryName = sc.Category!.CategoryName,
                    SubCategoryName = sc.SubCategoryName,
                    Description = sc.Description
                })
                .ToListAsync();
        }

        public async Task<SubCategoryResponseDTO?> GetSubCategoryByIdAsync(int id)
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .Where(sc => sc.SubCategoryId == id)
                .Select(sc => new SubCategoryResponseDTO
                {
                    SubCategoryId = sc.SubCategoryId,
                    CategoryId = sc.CategoryId,
                    CategoryName = sc.Category!.CategoryName,
                    SubCategoryName = sc.SubCategoryName,
                    Description = sc.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SubCategoryResponseDTO> CreateSubCategoryAsync(SubCategoryCreateDTO dto)
        {
            var subCategory = new SubCategory
            {
                CategoryId = dto.CategoryId,
                SubCategoryName = dto.SubCategoryName,
                Description = dto.Description
            };

            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();

            var category = await _context.Categories
                .FindAsync(dto.CategoryId);

            return new SubCategoryResponseDTO
            {
                SubCategoryId = subCategory.SubCategoryId,
                CategoryId = subCategory.CategoryId,
                CategoryName = category?.CategoryName ?? "",
                SubCategoryName = subCategory.SubCategoryName,
                Description = subCategory.Description
            };
        }

        public async Task<bool> UpdateSubCategoryAsync(int id, SubCategoryUpdateDTO dto)
        {
            var subCategory = await _context.SubCategories
                .FindAsync(id);

            if (subCategory == null)
                return false;

            subCategory.CategoryId = dto.CategoryId;
            subCategory.SubCategoryName = dto.SubCategoryName;
            subCategory.Description = dto.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSubCategoryAsync(int id)
        {
            var subCategory = await _context.SubCategories
                .FindAsync(id);

            if (subCategory == null)
                return false;

            _context.SubCategories.Remove(subCategory);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
