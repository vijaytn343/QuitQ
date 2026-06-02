using Microsoft.AspNetCore.Mvc;
using QuitQ.Services.Interfaces;

namespace QuitQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentById(int paymentId)
        {
            var payment = await _paymentService
                .GetPaymentByIdAsync(paymentId);

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrder(int orderId)
        {
            var payments = await _paymentService
                .GetPaymentsByOrderIdAsync(orderId);

            return Ok(payments);
        }
        [HttpPut("{paymentId}")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentId,string paymentStatus,string? transactionId)
        {
            var updated = await _paymentService
                .UpdatePaymentStatusAsync(
                    paymentId,
                    paymentStatus,
                    transactionId);

            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
