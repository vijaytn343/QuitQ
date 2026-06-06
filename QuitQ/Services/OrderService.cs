using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.OrderDTOs;
using QuitQ.Models;
using QuitQ.Services.Interfaces;

namespace QuitQ.Services
{
    public class OrderService:IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrderResponseDTO> CreateOrderAsync(int userId, CreateOrderDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get Cart
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                    throw new Exception("Cart is empty.");

                // Check Address
                var address = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.AddressId == dto.AddressId
                                           && a.UserId == userId);

                if (address == null)
                    throw new Exception("Address not found.");

                decimal totalAmount = 0;

                // Validate Stock
                foreach (var item in cart.CartItems)
                {
                    var inventory = await _context.Inventories
                        .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

                    if (inventory == null)
                        throw new Exception("Inventory not found.");

                    if (inventory.QuantityAvailable < item.Quantity)
                        throw new Exception(
                            $"Not enough stock for {item.Product!.ProductName}");

                    totalAmount += item.Product!.Price * item.Quantity;
                }

                // Create Order
                var order = new Order
                {
                    UserId = userId,
                    AddressId = dto.AddressId,
                    OrderStatus = "Pending",
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount
                };

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                // Create OrderItems + Reduce Inventory
                foreach (var item in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PriceAtPurchase = item.Product!.Price
                    };

                    _context.OrderItems.Add(orderItem);

                    var inventory = await _context.Inventories
                        .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

                    inventory!.QuantityAvailable -= item.Quantity;
                }

                // Create Payment
                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    PaymentMethod = dto.PaymentMethod,
                    Amount = totalAmount,
                    PaymentStatus = "Pending",
                    PaymentDate = DateTime.Now
                };

                _context.Payments.Add(payment);

                // Clear Cart
                _context.CartItems.RemoveRange(cart.CartItems);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetOrderByIdAsync(order.OrderId,userId)
                       ?? throw new Exception("Order creation failed.");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<OrderResponseDTO?> GetOrderByIdAsync(int orderId,int userId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o =>
    o.OrderId == orderId &&
    o.UserId == userId);

            if (order == null)
                return null;

            return new OrderResponseDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.User!.Name,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,

                FullAddress = order.Address!.FullAddress,
                City = order.Address.City,
                State = order.Address.State,
                Pincode = order.Address.Pincode,
                Country = order.Address.Country,

                OrderItems = order.OrderItems!
                    .Select(oi => new OrderItemResponseDTO
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product!.ProductName,
                        Quantity = oi.Quantity,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        SubTotal = oi.Quantity * oi.PriceAtPurchase
                    })
                    .ToList()
            };
        }
        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders.Select(order => new OrderResponseDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                CustomerName = order.User!.Name,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,

                FullAddress = order.Address!.FullAddress,
                City = order.Address.City,
                State = order.Address.State,
                Pincode = order.Address.Pincode,
                Country = order.Address.Country,

                OrderItems = order.OrderItems!
                    .Select(oi => new OrderItemResponseDTO
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product!.ProductName,
                        Quantity = oi.Quantity,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        SubTotal = oi.Quantity * oi.PriceAtPurchase
                    })
                    .ToList()
            });
        }
        public async Task<IEnumerable<SellerOrderResponseDTO>>GetSellerOrdersAsync(int userId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o!.User)
                .Include(oi => oi.Product)
                    .ThenInclude(p => p!.Seller)
                .Where(oi =>
                    oi.Product!.Seller!.UserId == userId)
                .Select(oi => new SellerOrderResponseDTO
                {
                    OrderId = oi.OrderId,
                    CustomerName = oi.Order!.User!.Name,
                    ProductName = oi.Product!.ProductName,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase,
                    OrderStatus = oi.Order.OrderStatus,
                    OrderDate = oi.Order.OrderDate
                })
                .ToListAsync();
        }
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders
                .FindAsync(orderId);

            if (order == null)
                return false;

            order.OrderStatus = status;
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
