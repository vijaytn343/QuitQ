using QuitQ.DTOs.SellerDTOs;

namespace QuitQ.Services.SellerFeature
{
    public interface ISellerService
    {
        Task<IEnumerable<SellerResponseDTO>> GetAllSellersAsync();

        Task<SellerResponseDTO?> GetSellerByIdAsync(int id);
        Task<List<SalesReportDTO>>
    GetSalesReportAsync(int userId);
        Task<SalesSummaryDTO> GetSalesSummaryAsync(int userId);

        Task<SellerResponseDTO> CreateSellerAsync(int userId,SellerCreateDTO dto);

        Task<bool> UpdateSellerByUserIdAsync(int userId,SellerUpdateDTO dto);

        Task<bool> DeleteSellerAsync(int id);
    }
}
