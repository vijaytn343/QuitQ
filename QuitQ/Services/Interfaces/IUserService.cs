using QuitQ.DTOs.UserDTOs;

namespace QuitQ.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();

        Task<UserResponseDTO?> GetUserByIdAsync(int id);

        Task<bool> UpdateUserAsync(int userId,int id, UserUpdateDTO dto);

        Task<bool> DeleteUserAsync(int id);
    }
}
