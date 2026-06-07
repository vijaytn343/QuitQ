using QuitQ.DTOs.CartDTOs;

namespace QuitQ.Services.CartFeature
{
    public interface ICartService
    {
        Task<CartResponseDTO?> GetCartByUserIdAsync(int userId);

        Task<CartResponseDTO> AddToCartAsync(int userId, AddToCartDTO dto);

        Task<bool> UpdateCartItemAsync(int userId,int cartItemId, UpdateCartItemDTO dto);

        Task<bool> RemoveCartItemAsync(int userId,int cartItemId);

        Task<bool> ClearCartAsync(int userId);
    }
}
