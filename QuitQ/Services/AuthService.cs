using QuitQ.Data;
using QuitQ.DTOs.AuthDTOs;
using QuitQ.Models;
using QuitQ.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace QuitQ.Services
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
                throw new Exception("Email already exists.");

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == dto.Role);

            if (role == null)
                throw new Exception(" role not found.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                RoleId = role.RoleId,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = role.RoleName,
                Message = "Registration successful"
            };
        }
        private string GenerateJwtToken(User user, string role)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier,
                  user.UserId.ToString()),

        new Claim(ClaimTypes.Name,
                  user.Name),

        new Claim(ClaimTypes.Email,
                  user.Email),

        new Claim(ClaimTypes.Role,
                  role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Invalid Email or Password");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Invalid Email or Password");

            string token = GenerateJwtToken(
                user,
                user.Role!.RoleName);

            return new AuthResponseDTO
            {
                Token = token,
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.RoleName,
                Message = "Login Successful"
            };
        }
    }
}
