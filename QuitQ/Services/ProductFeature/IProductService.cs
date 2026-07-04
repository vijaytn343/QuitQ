using QuitQ.DTOs.ProductDTOs;
using QuitQ.DTOs.Common;
namespace QuitQ.Services.ProductFeature
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync();
        Task<PagedResultDTO<ProductResponseDTO>>
 GetProductsAsync(ProductFilterDTO filter);
        Task<ProductResponseDTO?> GetProductByIdAsync(int id);
        Task<List<ProductResponseDTO>> GetMyProductsAsync(int userId);
        Task<IEnumerable<ProductResponseDTO>> GetRelatedProductsAsync(int productId);

        Task<ProductResponseDTO> CreateProductAsync(int userId,ProductCreateDTO dto);

        Task<bool> UpdateProductAsync(int userId,int productId, ProductUpdateDTO dto);

        Task<bool> DeleteProductAsync(int userId,int producId);
      
    }
}
