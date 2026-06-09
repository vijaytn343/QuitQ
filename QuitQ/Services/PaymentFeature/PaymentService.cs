using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.PaymentDTOs;
using QuitQ.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
namespace QuitQ.Services.PaymentFeature
{
    public class PaymentService: IPaymentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        public PaymentService(AppDbContext context,IMapper mapper, ILogger<PaymentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<PaymentResponseDTO?> GetPaymentByIdAsync(int paymentId,int userId)
        {
            var payment = await _context.Payments.Include(p=>p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId && p.Order!.UserId==userId);

            if (payment == null)
                return null;

            return _mapper.Map<PaymentResponseDTO>(payment);
        }
        public async Task<IEnumerable<PaymentResponseDTO>> GetPaymentsByOrderIdAsync(int orderId,int userId)
        {
            var payments = await _context.Payments.Include(p => p.Order).Where(p =>
         p.OrderId == orderId &&
         p.Order!.UserId == userId)
     .ToListAsync();

            return _mapper.Map<List<PaymentResponseDTO>>(payments);
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
            _logger.LogInformation(
    "Payment {PaymentId} updated to {Status}",
    paymentId,
    paymentStatus);

            return true;
        }
    }
}
