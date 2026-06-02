using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.PaymentDTOs;
using QuitQ.Models;
using QuitQ.Services.Interfaces;

namespace QuitQ.Services
{
    public class PaymentService: IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PaymentResponseDTO?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null)
                return null;

            return new PaymentResponseDTO
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId
            };
        }
        public async Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByOrderIdAsync(int orderId)
        {
            var payments = await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();

            return payments.Select(payment => new PaymentResponseDTO
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId
            });
        }
        public async Task<bool> UpdatePaymentStatusAsync(int paymentId,string paymentStatus,string? transactionId)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null)
                return false;

            payment.PaymentStatus = paymentStatus;

            if (!string.IsNullOrWhiteSpace(transactionId))
            {
                payment.TransactionId = transactionId;
            }

            payment.PaymentDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
