using QuitQ.Data;
using QuitQ.DTOs.CartDTOs;
using QuitQ.Models;
using Microsoft.EntityFrameworkCore;

namespace QuitQ.Services.CartFeature
{
    public class CartService:ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CartResponseDTO> AddToCartAsync(
    int userId,
    AddToCartDTO dto)
        {
            // Check User
            var user = await _context.Users
                .FindAsync(userId);

            if (user == null)
                throw new Exception("User not found.");

            // Check Product
            var product = await _context.Products
                .FindAsync(dto.ProductId);

            if (product == null)
                throw new Exception("Product not found.");

            // Check Inventory
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.ProductId == dto.ProductId);

            if (inventory == null)
                throw new Exception("Inventory not found.");

            if (dto.Quantity > inventory.QuantityAvailable)
                throw new Exception("Not enough stock available.");

            // Get/Create Cart
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(cart);

                await _context.SaveChangesAsync();
            }

            // Check existing CartItem
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cart.CartId &&
                    ci.ProductId == dto.ProductId);

            if (cartItem != null)
            {
                var newQuantity = cartItem.Quantity + dto.Quantity;

                if (newQuantity > inventory.QuantityAvailable)
                    throw new Exception("Not enough stock available.");

                cartItem.Quantity = newQuantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return await GetCartByUserIdAsync(userId)
                   ?? throw new Exception("Cart not found.");
        }
        public async Task<bool>
UpdateQuantityAsync(
    UpdateCartQuantityDTO dto)
        {
            var item =
                await _context.CartItems
                .Include(c => c.Product)
                .ThenInclude(p => p.Inventory)
                .FirstOrDefaultAsync(c =>
                    c.CartItemId ==
                    dto.CartItemId);

            if (item == null)
                return false;

            if (item.Product == null)
                return false;

            if (item.Product.Inventory == null)
                return false;

            if (dto.Quantity >
                item.Product.Inventory.QuantityAvailable)
                return false;

            item.Quantity =
                dto.Quantity;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<CartResponseDTO?> GetCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return null;

            var response = new CartResponseDTO
            {
                CartId = cart.CartId,
                UserId = cart.UserId
            };

            foreach (var item in cart.CartItems!)
            {
                response.CartItems.Add(new CartItemResponseDTO
                {
                    CartItemId = item.CartItemId,
                    ProductId = item.ProductId,
                    ProductName = item.Product!.ProductName,
                    ImageUrl = item.Product.ImageUrl,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    SubTotal = item.Product.Price * item.Quantity
                });
            }

            response.TotalAmount = response.CartItems
                .Sum(i => i.SubTotal);

            return response;
        }
        public async Task<bool> UpdateCartItemAsync(int userId,int cartItemId,UpdateCartItemDTO dto)
        {
            var cartItem = await _context.CartItems.Include(ci => ci.Cart).Include(ci => ci.Product)
        .ThenInclude(p => p!.Inventory).FirstOrDefaultAsync(ci =>
        ci.CartItemId == cartItemId &&
        ci.Cart!.UserId == userId);

            if (cartItem == null)
                return false;

            if (dto.Quantity > cartItem.Product!.Inventory!.QuantityAvailable)
                throw new Exception("Not enough stock available.");

            cartItem.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> RemoveCartItemAsync(int userId,int cartItemId)
        {
            var cartItem = await _context.CartItems.Include(ci => ci.Cart).FirstOrDefaultAsync(ci =>
         ci.CartItemId == cartItemId &&
         ci.Cart!.UserId == userId);

            if (cartItem == null)
                return false;

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return false;

            _context.CartItems.RemoveRange(cart.CartItems!);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
