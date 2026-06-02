using QuitQ.DTOs.PaymentDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface IPaymentService
    {
       
        Task<PaymentResponseDTO?> GetPaymentByIdAsync(int paymentId);

        Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByOrderIdAsync(int orderId);

        Task<bool> UpdatePaymentStatusAsync(int paymentId,string paymentStatus,string? transactionId);
    }
}
