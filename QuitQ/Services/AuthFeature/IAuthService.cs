using QuitQ.DTOs.AuthDTOs;

namespace QuitQ.Services.AuthFeature
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);

        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
    }
}
