using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.UserDTOs;

namespace QuitQ.Services.UserFeature
{
    public class UserService:IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Gender = u.Gender,
                    RoleName = u.Role!.RoleName,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }
        public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.UserId == id)
                .Select(u => new UserResponseDTO
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Gender = u.Gender,
                    RoleName = u.Role!.RoleName,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateUserAsync(int userId,int id, UserUpdateDTO dto)
        {
            var user = await _context.Users
     .FirstOrDefaultAsync(u =>
         u.UserId == id &&
         u.UserId == userId);

            if (user == null)
                return false;

            user.Name = dto.Name;
            user.Phone = dto.Phone;
            user.Gender = dto.Gender;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            user.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
