using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.CartDTOs;
using QuitQ.Services.Interfaces;

namespace QuitQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddToCart(
    int userId,
    AddToCartDTO dto)
        {
            var cart = await _cartService.AddToCartAsync(userId, dto);

            return Ok(cart);
        }
        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(
    int cartItemId,
    UpdateCartItemDTO dto)
        {
            var updated = await _cartService
                .UpdateCartItemAsync(cartItemId, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var removed = await _cartService
                .RemoveCartItemAsync(cartItemId);

            if (!removed)
                return NotFound();

            return NoContent();
        }
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            var cleared = await _cartService
                .ClearCartAsync(userId);

            if (!cleared)
                return NotFound();

            return NoContent();
        }
    }
}
