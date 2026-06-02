using QuitQ.DTOs.CartDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartResponseDTO?> GetCartByUserIdAsync(int userId);

        Task<CartResponseDTO> AddToCartAsync(int userId, AddToCartDTO dto);

        Task<bool> UpdateCartItemAsync(int cartItemId, UpdateCartItemDTO dto);

        Task<bool> RemoveCartItemAsync(int cartItemId);

        Task<bool> ClearCartAsync(int userId);
    }
}
