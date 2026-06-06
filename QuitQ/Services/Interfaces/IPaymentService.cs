using QuitQ.DTOs.PaymentDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface IPaymentService
    {
       
        Task<PaymentResponseDTO?> GetPaymentByIdAsync(int paymentId,int userId);

        Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByOrderIdAsync(int orderId,int userId);

        Task<bool> UpdatePaymentStatusAsync(int paymentId,string paymentStatus,string? transactionId);
    }
}
