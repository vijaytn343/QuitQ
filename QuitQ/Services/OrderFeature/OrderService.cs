using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;
using QuitQ.Data;
using QuitQ.Documents;
using QuitQ.DTOs.OrderDTOs;
using QuitQ.DTOs.SellerDTOs;
using QuitQ.Models;
using QuitQ.Services.EmailFeature;
using QuitQ.Services.OrderFeature;

namespace QuitQ.Services.OrderFeature
{
    public class OrderService:IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;
        private readonly IEmailService _emailService;

        public OrderService( AppDbContext context,IMapper mapper, ILogger<OrderService> logger, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
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
                var user = await _context.Users
    .FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                    throw new Exception("User not found.");

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
                _logger.LogInformation(
    "Order {OrderId} created by User {UserId}",
    order.OrderId,
    userId);
                try
                {
                    await _emailService.SendEmailAsync(
                        user.Email,
                        "Order Confirmation - QuitQ",
                        $@"
        <h2>Order Confirmed</h2>
        <p>Hello {user.Name},</p>
        <p>Your order #{order.OrderId} has been placed successfully.</p>
        <p>Total Amount: ₹{order.TotalAmount}</p>
        <p>Status: {order.OrderStatus}</p>
        <p>Thank you for shopping with QuitQ.</p>
        ");
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to send order confirmation email for Order {OrderId}",
                        order.OrderId);
                }

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

            return _mapper.Map<OrderResponseDTO>(order);
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

            return _mapper.Map<List<OrderResponseDTO>>(orders);
        }
        public async Task<byte[]> GenerateInvoiceAsync(
    int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(
                    o => o.OrderId == orderId);

            if (order == null)
                throw new Exception("Order not found");

            var document =
                new InvoiceDocument(order);

            return document.GeneratePdf();
        }
        public async Task<IEnumerable<SellerOrderResponseDTO>>GetSellerOrdersAsync(int userId)
        {
            var orderItems = await _context.OrderItems
        .Include(oi => oi.Order)
            .ThenInclude(o => o!.User)
        .Include(oi => oi.Product)
            .ThenInclude(p => p!.Seller)
        .Where(oi =>
            oi.Product!.Seller!.UserId == userId)
        .ToListAsync();

            return _mapper.Map<List<SellerOrderResponseDTO>>(orderItems);
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
            _logger.LogInformation(
    "Order {OrderId} status updated to {Status}",
    orderId,
    status);
            return true;
        }
        public async Task<SellerDashboardDTO>
    GetSellerDashboardAsync(int userId)
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Product)
                    .ThenInclude(p => p!.Seller)
                .Where(oi =>
                    oi.Product!.Seller!.UserId == userId)
                .ToListAsync();

            return new SellerDashboardDTO
            {
                TotalOrders = orderItems
                    .Select(oi => oi.OrderId)
                    .Distinct()
                    .Count(),

                TotalProductsSold = orderItems
                    .Sum(oi => oi.Quantity),

                TotalRevenue = orderItems
                    .Sum(oi =>
                        oi.Quantity * oi.PriceAtPurchase)
            };
        }
    }
}
