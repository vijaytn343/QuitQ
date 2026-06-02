using QuitQ.DTOs.CategoryDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDTO>> GetAllCategoriesAsync();

        Task<CategoryResponseDTO?> GetCategoryByIdAsync(int id);

        Task<CategoryResponseDTO> CreateCategoryAsync(CategoryCreateDTO dto);

        Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDTO dto);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
