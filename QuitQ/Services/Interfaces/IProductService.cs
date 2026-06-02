using QuitQ.DTOs.ProductDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync();

        Task<ProductResponseDTO?> GetProductByIdAsync(int id);

        Task<ProductResponseDTO> CreateProductAsync(ProductCreateDTO dto);

        Task<bool> UpdateProductAsync(int id, ProductUpdateDTO dto);

        Task<bool> DeleteProductAsync(int id);
    }
}
