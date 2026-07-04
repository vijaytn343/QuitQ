using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QuitQ.Data;
using QuitQ.DTOs.AuthDTOs;
using QuitQ.Models;
using QuitQ.Services.EmailFeature;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuitQ.Services.AuthFeature
{
    public class AuthService:IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IEmailService _emailService;
        public AuthService( AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
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
            _logger.LogInformation(
    "User {Email} registered successfully",
    user.Email);
            try
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Welcome to QuitQ",
                    $@"
        <h2>Welcome to QuitQ</h2>
        <p>Hello {user.Name},</p>
        <p>Your account has been created successfully.</p>
        <p>Thank you for joining QuitQ.</p>
        ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to send welcome email to {Email}",
                    user.Email);
            }

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
        private string GeneratePasswordResetToken(User user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("Purpose","PasswordReset")
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Invalid Email or Password");
            if (!user.IsActive)
                throw new Exception("Account is deactivated.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Invalid Email or Password");

            string token = GenerateJwtToken(
                user,
                user.Role!.RoleName);
            _logger.LogInformation(
    "User {Email} logged in successfully",
    user.Email);

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
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            // Don't reveal whether the email exists
            if (user == null)
                return;

            string token = GeneratePasswordResetToken(user);

            string resetLink =
                $"http://localhost:5173/reset-password?token={token}";

            await _emailService.SendEmailAsync(
                user.Email,
                "Reset Your QuitQ Password",
                $@"
        <h2>Reset Password</h2>

        <p>Hello {user.Name},</p>

        <p>We received a request to reset your QuitQ password.</p>

        <p>
            <a href='{resetLink}'
               style='background:#2563eb;
                      color:white;
                      padding:10px 20px;
                      text-decoration:none;
                      border-radius:5px;'>
                Reset Password
            </a>
        </p>

        <p>This link expires in 15 minutes.</p>

        <p>If you didn't request this, you can safely ignore this email.</p>
        ");
        }
        public async Task ResetPasswordAsync(string token, string newPassword)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!))
            };

            var principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out _);

            var purpose = principal.FindFirst("Purpose")?.Value;

            if (purpose != "PasswordReset")
                throw new Exception("Invalid password reset token.");

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new Exception("Invalid token.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new Exception("User not found.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _context.SaveChangesAsync();
        }
    }
}
