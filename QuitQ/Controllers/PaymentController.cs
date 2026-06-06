using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.Services.Interfaces;
using System.Security.Claims;

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
        [Authorize]
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentById(int paymentId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var payment = await _paymentService .GetPaymentByIdAsync(paymentId, userId);

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }
        [Authorize]
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrder(int orderId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var payments = await _paymentService
                .GetPaymentsByOrderIdAsync(orderId, userId);

            return Ok(payments);
        }
        [Authorize(Roles = "Admin")]
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
