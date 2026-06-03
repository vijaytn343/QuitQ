using QuitQ.DTOs.AuthDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);

        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
    }
}
