using QuitQ.DTOs.AddressDTOs;

namespace QuitQ.Services.AddressService
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressResponseDTO>> GetAllAddressesAsync();

        Task<AddressResponseDTO?> GetAddressByIdAsync(int userId); 
        Task<IEnumerable<AddressResponseDTO>>
GetMyAddressesAsync(int userId);

        Task<AddressResponseDTO> CreateAddressAsync(int userId, AddressCreateDTO dto);

        Task<bool> UpdateAddressAsync( int userId,int addressId,AddressUpdateDTO dto);

        Task<bool> DeleteAddressAsync(int userId, int addressId);
    }
}
