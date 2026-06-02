using QuitQ.DTOs.AddressDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressResponseDTO>> GetAllAddressesAsync();

        Task<AddressResponseDTO?> GetAddressByIdAsync(int id);

        Task<AddressResponseDTO> CreateAddressAsync(int userId, AddressCreateDTO dto);

        Task<bool> UpdateAddressAsync(int id, AddressUpdateDTO dto);

        Task<bool> DeleteAddressAsync(int id);
    }
}
