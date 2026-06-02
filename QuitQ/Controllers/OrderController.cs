using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.OrderDTOs;
using QuitQ.Services.Interfaces;

namespace QuitQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateOrder(int userId,CreateOrderDTO dto)
        {
            var order = await _orderService
                .CreateOrderAsync(userId, dto);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { orderId = order.OrderId },
                order);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService
                .GetOrderByIdAsync(orderId);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await _orderService
                .GetOrdersByUserIdAsync(userId);

            return Ok(orders);
        }
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
