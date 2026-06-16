using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.AdminDTO;

namespace QuitQ.Services.AdminFeature
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardDTO> GetDashboardAsync()
        {
            return new AdminDashboardDTO
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalSellers = await _context.Sellers.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount)
            };
        }
    }
}

