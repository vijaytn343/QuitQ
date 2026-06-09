using QuitQ.Data;
using QuitQ.DTOs.CategoryDTOs;
using QuitQ.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace QuitQ.Services.CategoryFeature
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService( AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            return _mapper.Map<List<CategoryResponseDTO>>(categories);
        }

        public async Task<CategoryResponseDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
      .FirstOrDefaultAsync(c => c.CategoryId == id);

            return category == null? null: _mapper.Map<CategoryResponseDTO>(category);
        }

        public async Task<CategoryResponseDTO> CreateCategoryAsync(CategoryCreateDTO dto)
        {
            var category = _mapper.Map<Category>(dto);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryResponseDTO>(category);
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return false;

            _mapper.Map(dto, category);

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
