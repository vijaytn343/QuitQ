using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.CartDTOs;
using QuitQ.Services.CartFeature;
using System.Security.Claims;
namespace QuitQ.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await _cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cart = await _cartService.AddToCartAsync(userId, dto);

            return Ok(cart);
        }
        [HttpPut("update-quantity")]
        public async Task<IActionResult>
UpdateQuantity(
    UpdateCartQuantityDTO dto)
        {
            var result =
                await _cartService
                .UpdateQuantityAsync(dto);

            if (!result)
                return BadRequest(
                    "Stock limit reached");

            return NoContent();
        }
        [Authorize]
        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId,UpdateCartItemDTO dto)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

            var updated = await _cartService
                .UpdateCartItemAsync(
                    userId,
                    cartItemId,
                    dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        [Authorize]
        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

            var removed = await _cartService
                .RemoveCartItemAsync(
                    userId,
                    cartItemId);

            if (!removed)
                return NotFound();

            return NoContent();
        }
        [Authorize]
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var cleared = await _cartService
                .ClearCartAsync(userId);

            if (!cleared)
                return NotFound();

            return NoContent();
        }
    }
}
