using QuitQ.DTOs.OrderDTOs;
using QuitQ.DTOs.SellerDTOs;

namespace QuitQ.Services.OrderFeature
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(int userId, CreateOrderDTO dto);

        Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId);
        Task<byte[]> GenerateInvoiceAsync(int orderId);
        Task<OrderResponseDTO?> GetOrderByIdAsync(int orderId,int userId);
        Task<SellerDashboardDTO> GetSellerDashboardAsync(int userId);
        Task<IEnumerable<SellerOrderResponseDTO>> GetSellerOrdersAsync(int userId);

        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
