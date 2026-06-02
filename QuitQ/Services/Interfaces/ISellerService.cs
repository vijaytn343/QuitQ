using QuitQ.DTOs.SellerDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface ISellerService
    {
        Task<IEnumerable<SellerResponseDTO>> GetAllSellersAsync();

        Task<SellerResponseDTO?> GetSellerByIdAsync(int id);

        Task<SellerResponseDTO> CreateSellerAsync(SellerCreateDTO dto);

        Task<bool> UpdateSellerAsync(int id, SellerUpdateDTO dto);

        Task<bool> DeleteSellerAsync(int id);
    }
}
