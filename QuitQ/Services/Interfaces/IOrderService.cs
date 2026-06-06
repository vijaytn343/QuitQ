using QuitQ.DTOs.OrderDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(int userId, CreateOrderDTO dto);

        Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId);

        Task<OrderResponseDTO?> GetOrderByIdAsync(int orderId,int userId);
        Task<IEnumerable<SellerOrderResponseDTO>> GetSellerOrdersAsync(int userId);

        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
