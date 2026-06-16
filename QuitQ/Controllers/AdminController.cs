using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.Services.AdminFeature;
using QuitQ.Services.SellerFeature;
using QuitQ.Services.UserFeature;

namespace QuitQ.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

       
        private readonly IAdminService _adminService;

        public AdminController(
            IUserService userService,
            IAdminService adminService)
        {
            _userService = userService;
            _adminService = adminService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
                return NotFound("User not found");

            return Ok("User deactivated successfully");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("sellers")]
        public async Task<IActionResult> GetAllSellers(
    [FromServices] ISellerService sellerService)
        {
            var sellers = await sellerService.GetAllSellersAsync();

            return Ok(sellers);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("sellers/{id}")]
        public async Task<IActionResult> DeleteSeller(
    int id,
    [FromServices] ISellerService sellerService)
        {
            var deleted = await sellerService.DeleteSellerAsync(id);

            if (!deleted)
                return NotFound("Seller not found");

            return Ok("Seller deleted successfully");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard =
                await _adminService.GetDashboardAsync();

            return Ok(dashboard);
        }
    }
}
