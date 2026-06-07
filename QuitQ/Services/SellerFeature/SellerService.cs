using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.SellerDTOs;
using QuitQ.Models;

namespace QuitQ.Services.SellerFeature
{
    public class SellerService : ISellerService
    {
        private readonly AppDbContext _context;

        public SellerService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SellerResponseDTO>> GetAllSellersAsync()
        {
            return await _context.Sellers
                .Include(s => s.User)
                .Select(s => new SellerResponseDTO
                {
                    SellerId = s.SellerId,
                    UserId = s.UserId,
                    StoreName = s.StoreName,
                    GSTNumber = s.GSTNumber,
                    BusinessEmail = s.BusinessEmail,
                    AccountHolderName = s.AccountHolderName,
                    AccountNumber = s.AccountNumber,
                    IFSCCode = s.IFSCCode,
                    BankName = s.BankName,
                    UserName = s.User!.Name
                })
                .ToListAsync();
        }
        public async Task<SellerResponseDTO?> GetSellerByIdAsync(int id)
        {
            return await _context.Sellers
                .Include(s => s.User)
                .Where(s => s.SellerId == id)
                .Select(s => new SellerResponseDTO
                {
                    SellerId = s.SellerId,
                    UserId = s.UserId,
                    StoreName = s.StoreName,
                    GSTNumber = s.GSTNumber,
                    BusinessEmail = s.BusinessEmail,
                    AccountHolderName = s.AccountHolderName,
                    AccountNumber = s.AccountNumber,
                    IFSCCode = s.IFSCCode,
                    BankName = s.BankName,
                    UserName = s.User!.Name
                })
                .FirstOrDefaultAsync();
        }
        public async Task<SellerResponseDTO> CreateSellerAsync(int userId,SellerCreateDTO dto)
        {
            var existingSeller = await _context.Sellers
       .FirstOrDefaultAsync(s => s.UserId == userId);

            if (existingSeller != null)
                throw new Exception("Seller profile already exists.");
            var seller = new Seller
            {
                UserId = userId,
                StoreName = dto.StoreName,
                GSTNumber = dto.GSTNumber,
                BusinessEmail = dto.BusinessEmail,
                AccountHolderName = dto.AccountHolderName,
                AccountNumber = dto.AccountNumber,
                IFSCCode = dto.IFSCCode,
                BankName = dto.BankName
            };

            _context.Sellers.Add(seller);

            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);

            return new SellerResponseDTO
            {
                SellerId = seller.SellerId,
                UserId = seller.UserId,
                StoreName = seller.StoreName,
                GSTNumber = seller.GSTNumber,
                BusinessEmail = seller.BusinessEmail,
                AccountHolderName = seller.AccountHolderName,
                AccountNumber = seller.AccountNumber,
                IFSCCode = seller.IFSCCode,
                BankName = seller.BankName,
                UserName = user?.Name
            };
        }
        public async Task<bool> UpdateSellerByUserIdAsync(int userId,SellerUpdateDTO dto)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (seller == null)
                return false;

            seller.StoreName = dto.StoreName;
            seller.GSTNumber = dto.GSTNumber;
            seller.BusinessEmail = dto.BusinessEmail;
            seller.AccountHolderName = dto.AccountHolderName;
            seller.AccountNumber = dto.AccountNumber;
            seller.IFSCCode = dto.IFSCCode;
            seller.BankName = dto.BankName;

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
