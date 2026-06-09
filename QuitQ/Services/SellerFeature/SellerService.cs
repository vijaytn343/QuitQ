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
