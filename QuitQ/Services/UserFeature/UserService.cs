using Microsoft.EntityFrameworkCore;
using QuitQ.Data;
using QuitQ.DTOs.UserDTOs;
using AutoMapper;
namespace QuitQ.Services.UserFeature
{
    public class UserService:IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users
    .Include(u => u.Role)
    .Where(u => u.Role.RoleName == "Customer" && u.IsActive)
    .ToListAsync();

            return _mapper.Map<List<UserResponseDTO>>(users);
        }
        public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.UserId == id);

            return user == null
                ? null
                : _mapper.Map<UserResponseDTO>(user);
        }
        public async Task<bool> UpdateUserAsync(int userId,int id, UserUpdateDTO dto)
        {
            var user = await _context.Users
     .FirstOrDefaultAsync(u =>
         u.UserId == id &&
         u.UserId == userId);

            if (user == null)
                return false;

            _mapper.Map(dto, user);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users
    .FirstOrDefaultAsync(u =>
        u.UserId == id &&
        u.IsActive);

            if (user == null)
                return false;

            user.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
