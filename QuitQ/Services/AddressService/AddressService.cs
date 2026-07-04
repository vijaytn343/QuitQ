using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.Models;
using QuitQ.DTOs.AddressDTOs;
using AutoMapper;

using Microsoft.Extensions.Logging;
namespace QuitQ.Services.AddressService
{
    public class AddressService:IAddressService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressService> _logger;

        public AddressService( AppDbContext context, IMapper mapper, ILogger<AddressService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<AddressResponseDTO>> GetAllAddressesAsync()
        {
            var addresses = await _context.Addresses
    .Include(a => a.User)
    .ToListAsync();

            return _mapper.Map<List<AddressResponseDTO>>(addresses);
        }
        public async Task<IEnumerable<AddressResponseDTO>>
GetMyAddressesAsync(int userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<AddressResponseDTO>>(addresses);
        }
        public async Task<AddressResponseDTO?> GetAddressByIdAsync(int userId)
        {
            var address = await _context.Addresses
    .Include(a => a.User)
    .FirstOrDefaultAsync(a => a.UserId == userId);

            return address == null? null : _mapper.Map<AddressResponseDTO>(address);
        }
        public async Task<AddressResponseDTO> CreateAddressAsync(
    int userId,
    AddressCreateDTO dto)
        {
            var address = _mapper.Map<Address>(dto);

            address.UserId = userId;

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();
            _logger.LogInformation(
    "Address created by User {UserId}",
    userId);

            var savedAddress = await _context.Addresses
     .Include(a => a.User)
     .FirstOrDefaultAsync(a => a.AddressId == address.AddressId);

            if (savedAddress == null)
                throw new Exception("Address not found after creation.");

            return _mapper.Map<AddressResponseDTO>(savedAddress);
        }
        public async Task<bool> UpdateAddressAsync(int userId,int addressId,AddressUpdateDTO dto)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a =>a.AddressId == addressId &&
         a.UserId == userId);

            if (address == null)
                return false;
            _mapper.Map(dto, address);
            await _context.SaveChangesAsync();
            _logger.LogInformation(
    "Address {AddressId} updated by User {UserId}",
    addressId,
    userId);

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
            _logger.LogInformation(
    "Address {AddressId} deleted by User {UserId}",
    addressId,
    userId);

            return true;
        }
    }
}
