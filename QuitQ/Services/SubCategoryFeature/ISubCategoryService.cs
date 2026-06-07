using QuitQ.DTOs.SubCategoryDTO;

namespace QuitQ.Services.SubCategoryFeature
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryResponseDTO>> GetAllSubCategoriesAsync();

        Task<SubCategoryResponseDTO?> GetSubCategoryByIdAsync(int id);

        Task<SubCategoryResponseDTO> CreateSubCategoryAsync(SubCategoryCreateDTO dto);

        Task<bool> UpdateSubCategoryAsync(int id, SubCategoryUpdateDTO dto);

        Task<bool> DeleteSubCategoryAsync(int id);
    }
}
