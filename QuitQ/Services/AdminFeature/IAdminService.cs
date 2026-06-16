using QuitQ.DTOs.AdminDTO;

namespace QuitQ.Services.AdminFeature
{
    public interface IAdminService
    {
        Task<AdminDashboardDTO> GetDashboardAsync();
    }
}
