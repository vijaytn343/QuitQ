using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.AddressDTOs;
using QuitQ.Models;
using QuitQ.Services.Interfaces;

namespace QuitQ.Services
{
    public class AddressService:IAddressService
    {
        private readonly AppDbContext _context;

        public AddressService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AddressResponseDTO>> GetAllAddressesAsync()
        {
            return await _context.Addresses
                .Include(a => a.User)
                .Select(a => new AddressResponseDTO
                {
                    AddressId = a.AddressId,
                    UserId = a.UserId,
                    UserName = a.User!.Name,
                    FullAddress = a.FullAddress,
                    City = a.City,
                    State = a.State,
                    Pincode = a.Pincode,
                    Country = a.Country
                })
                .ToListAsync();
        }
        public async Task<AddressResponseDTO?> GetAddressByIdAsync(int addressId,int userId)
        {
            return await _context.Addresses
                .Include(a => a.User)
               .Where(a =>a.AddressId == addressId && a.UserId == userId)
                .Select(a => new AddressResponseDTO
                {
                    AddressId = a.AddressId,
                    UserId = a.UserId,
                    UserName = a.User!.Name,
                    FullAddress = a.FullAddress,
                    City = a.City,
                    State = a.State,
                    Pincode = a.Pincode,
                    Country = a.Country
                })
                .FirstOrDefaultAsync();
        }
        public async Task<AddressResponseDTO> CreateAddressAsync(
    int userId,
    AddressCreateDTO dto)
        {
            var address = new Address
            {
                UserId = userId,
                FullAddress = dto.FullAddress,
                City = dto.City,
                State = dto.State,
                Pincode = dto.Pincode,
                Country = dto.Country
            };

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);

            return new AddressResponseDTO
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                UserName = user?.Name,
                FullAddress = address.FullAddress,
                City = address.City,
                State = address.State,
                Pincode = address.Pincode,
                Country = address.Country
            };
        }
        public async Task<bool> UpdateAddressAsync(int userId,int addressId,AddressUpdateDTO dto)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a =>a.AddressId == addressId &&
         a.UserId == userId);

            if (address == null)
                return false;

            address.FullAddress = dto.FullAddress;
            address.City = dto.City;
            address.State = dto.State;
            address.Pincode = dto.Pincode;
            address.Country = dto.Country;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteAddressAsync(int userId,int addressId)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a =>a.AddressId == addressId &&
        a.UserId == userId);

            if (address == null)
                return false;

            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
