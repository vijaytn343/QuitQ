using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuitQ.DTOs.AuthDTOs;
using QuitQ.Services.AuthFeature;
using System.Text;

namespace QuitQ.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(result);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
    ForgotPasswordDTO dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);

            return Ok(new
            {
                Message = "If an account with that email exists, a password reset link has been sent."
            });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
    ResetPasswordDTO dto)
        {
            await _authService.ResetPasswordAsync(
                dto.Token,
                dto.NewPassword);

            return Ok(new
            {
                Message = "Password reset successful."
            });
        }
    }
}
