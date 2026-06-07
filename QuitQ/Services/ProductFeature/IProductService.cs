using QuitQ.DTOs.ProductDTOs;

namespace QuitQ.Services.ProductFeature
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync();

        Task<ProductResponseDTO?> GetProductByIdAsync(int id);

        Task<ProductResponseDTO> CreateProductAsync(int userId,ProductCreateDTO dto);

        Task<bool> UpdateProductAsync(int userId,int productId, ProductUpdateDTO dto);

        Task<bool> DeleteProductAsync(int userId,int producId);
    }
}
