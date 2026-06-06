using QuitQ.DTOs.SellerDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface ISellerService
    {
        Task<IEnumerable<SellerResponseDTO>> GetAllSellersAsync();

        Task<SellerResponseDTO?> GetSellerByIdAsync(int id);

        Task<SellerResponseDTO> CreateSellerAsync(int userId,SellerCreateDTO dto);

        Task<bool> UpdateSellerByUserIdAsync(int userId,SellerUpdateDTO dto);

        Task<bool> DeleteSellerAsync(int id);
    }
}
