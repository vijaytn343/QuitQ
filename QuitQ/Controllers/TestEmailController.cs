using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using QuitQ.Services.EmailFeature;

namespace QuitQ.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public TestEmailController(
            IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail()
        {
            await _emailService.SendEmailAsync(
                "vijaytn343@gmail.com",
                "QuitQ Test Email",
                "<h2>Email service is working!</h2>");

            return Ok("Email sent successfully");
        }
    }
}
