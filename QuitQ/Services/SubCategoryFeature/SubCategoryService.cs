using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.SubCategoryDTO;
using QuitQ.Models;
using AutoMapper;
namespace QuitQ.Services.SubCategoryFeature
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SubCategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SubCategoryResponseDTO>> GetAllSubCategoriesAsync()
        {
            var subCategories = await _context.SubCategories.Include(sc => sc.Category)
    .ToListAsync();

            return _mapper.Map<List<SubCategoryResponseDTO>>(subCategories);
        }

        public async Task<SubCategoryResponseDTO?> GetSubCategoryByIdAsync(int id)
        {
            var subCategory = await _context.SubCategories.Include(sc => sc.Category)
     .FirstOrDefaultAsync(sc => sc.SubCategoryId == id);

            return subCategory == null? null: _mapper.Map<SubCategoryResponseDTO>(subCategory);
        }

        public async Task<SubCategoryResponseDTO> CreateSubCategoryAsync(SubCategoryCreateDTO dto)
        {
            var subCategory = _mapper.Map<SubCategory>(dto);

            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();

            var savedSubCategory = await _context.SubCategories
    .Include(sc => sc.Category)
    .FirstOrDefaultAsync(sc =>
        sc.SubCategoryId == subCategory.SubCategoryId);

            if (savedSubCategory == null)
                throw new Exception("SubCategory not found after creation.");

            return _mapper.Map<SubCategoryResponseDTO>(savedSubCategory);
        }

        public async Task<bool> UpdateSubCategoryAsync(int id, SubCategoryUpdateDTO dto)
        {
            var subCategory = await _context.SubCategories
                .FindAsync(id);

            if (subCategory == null)
                return false;

            _mapper.Map(dto, subCategory);
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
