using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.OrderDTOs;
using QuitQ.Services.OrderFeature;
using System.Security.Claims;

namespace QuitQ.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO dto)
        {
            var userId = int.Parse(
       User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var order = await _orderService
                .CreateOrderAsync(userId, dto);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { orderId = order.OrderId },
                order);
        }
        [Authorize]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

            var order = await _orderService
                .GetOrderByIdAsync(orderId, userId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
        [Authorize]
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = await _orderService
                .GetOrdersByUserIdAsync(userId);

            return Ok(orders);
        }
        [Authorize(Roles = "Seller")]
        [HttpGet("seller-orders")]
        public async Task<IActionResult> GetSellerOrders()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

            var orders = await _orderService
                .GetSellerOrdersAsync(userId);

            return Ok(orders);
        }
        [HttpGet("invoice/{orderId}")]
        public async Task<IActionResult> DownloadInvoice(
    int orderId)
        {
            var pdf =
                await _orderService
                    .GenerateInvoiceAsync(orderId);

            return File(
                pdf,
                "application/pdf",
                $"Invoice_{orderId}.pdf");
        }
        [Authorize(Roles = "Seller")]
        [HttpGet("seller-dashboard")]
        public async Task<IActionResult> GetSellerDashboard()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var dashboard =
                await _orderService.GetSellerDashboardAsync(userId);

            return Ok(dashboard);
        }
        [Authorize(Roles = "Seller,Admin")]
        [HttpPut("{orderId}/status")]
        public async Task<IActionResult> UpdateStatus(
    int orderId,
    string status)
        {
            var updated = await _orderService
                .UpdateOrderStatusAsync(orderId, status);

            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
