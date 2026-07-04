using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.SellerDTOs;
using QuitQ.Models;
using AutoMapper;

namespace QuitQ.Services.SellerFeature
{
    public class SellerService : ISellerService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SellerService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SellerResponseDTO>> GetAllSellersAsync()
        {
            var sellers = await _context.Sellers
     .Include(s => s.User)
     .ToListAsync();

            return _mapper.Map<List<SellerResponseDTO>>(sellers);
        }
        public async Task<SellerResponseDTO?> GetSellerByIdAsync(int id)
        {
            var seller = await _context.Sellers
    .Include(s => s.User)
    .FirstOrDefaultAsync(s => s.SellerId == id);

            return seller == null? null: _mapper.Map<SellerResponseDTO>(seller);
        }
        public async Task<List<SalesReportDTO>>
    GetSalesReportAsync(int userId)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(
                    s => s.UserId == userId);

            if (seller == null)
                return new List<SalesReportDTO>();

            var report = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi =>
                    oi.Product.SellerId ==
                    seller.SellerId)
                .GroupBy(oi =>
                    oi.Product.ProductName)
                .Select(g =>
                    new SalesReportDTO
                    {
                        ProductName = g.Key,

                        QuantitySold =
                            g.Sum(x => x.Quantity),

                        Revenue =
                            g.Sum(x =>
                                x.Quantity *
                                x.PriceAtPurchase)
                    })
                .ToListAsync();

            return report;
        }
        public async Task<SalesSummaryDTO>
    GetSalesSummaryAsync(int userId)
        {
            var seller =
                await _context.Sellers
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId);

            if (seller == null)
                return new SalesSummaryDTO();
            var orderItems =
                await _context.OrderItems
                .Include(o => o.Product)
                .Include(o => o.Order)
                .Where(o =>
                    o.Product.SellerId ==
                    seller.SellerId)
                .ToListAsync();

            var totalRevenue =
     orderItems.Sum(x =>
         x.Quantity * x.PriceAtPurchase);

            var productsSold =
                orderItems.Sum(x =>
                    x.Quantity);

            var weekStart =
                DateTime.Now.Date.AddDays(-7);

            var weeklyRevenue =
                orderItems
                .Where(x =>
                    x.Order != null &&
                    x.Order.OrderDate >= weekStart)
                .Sum(x =>
                    x.Quantity * x.PriceAtPurchase);

            var monthlyRevenue =
                orderItems
                .Where(x =>
                    x.Order != null &&
                    x.Order.OrderDate.Month ==
                    DateTime.Now.Month &&
                    x.Order.OrderDate.Year ==
                    DateTime.Now.Year)
                .Sum(x =>
                    x.Quantity * x.PriceAtPurchase);

            var monthlyTopProduct =
      orderItems
      .Where(x =>
          x.Order != null &&
          x.Order.OrderDate.Month ==
          DateTime.Now.Month &&
          x.Order.OrderDate.Year ==
          DateTime.Now.Year)
      .GroupBy(x => x.Product.ProductName)
      .OrderByDescending(g =>
          g.Sum(x => x.Quantity))
      .Select(g => g.Key)
      .FirstOrDefault() ?? "N/A";

            return new SalesSummaryDTO
            {
                TotalRevenue = totalRevenue,
                WeeklyRevenue = weeklyRevenue,
                MonthlyRevenue = monthlyRevenue,
                ProductsSold = productsSold,
                MonthlyTopProduct = monthlyTopProduct
            };
        }
        public async Task<SellerResponseDTO> CreateSellerAsync(int userId,SellerCreateDTO dto)
        {
            var existingSeller = await _context.Sellers
       .FirstOrDefaultAsync(s => s.UserId == userId);

            if (existingSeller != null)
                throw new Exception("Seller profile already exists.");
            var seller = _mapper.Map<Seller>(dto);

            seller.UserId = userId;

            _context.Sellers.Add(seller);

            await _context.SaveChangesAsync();

            var savedSeller = await _context.Sellers
     .Include(s => s.User)
     .FirstOrDefaultAsync(s => s.SellerId == seller.SellerId);

            if (savedSeller == null)
                throw new Exception("Seller not found after creation.");

            return _mapper.Map<SellerResponseDTO>(savedSeller);
        }
        public async Task<bool> UpdateSellerByUserIdAsync(int userId,SellerUpdateDTO dto)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                return false;

            _mapper.Map(dto, seller);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteSellerAsync(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);

            if (seller == null)
                return false;

            _context.Sellers.Remove(seller);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
