using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.SellerDTOs;
using QuitQ.Services.SellerFeature;
using System.Security.Claims;
namespace QuitQ.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sellers = await _sellerService.GetAllSellersAsync();
            return Ok(sellers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seller = await _sellerService.GetSellerByIdAsync(id);

            if (seller == null)
                return NotFound();

            return Ok(seller);
        }
        [Authorize(Roles = "Seller")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var sellers = await _sellerService.GetAllSellersAsync();

            var seller = sellers.FirstOrDefault(
                s => s.UserId == userId);

            if (seller == null)
                return NotFound();

            return Ok(seller);
        }
        [Authorize(Roles = "Seller")]
        [HttpGet("sales-report")]
        public async Task<IActionResult>
    GetSalesReport()
        {
            var userId =
     int.Parse(
         User.FindFirst(
             ClaimTypes.NameIdentifier
         )!.Value);
            var report =
                await _sellerService
                    .GetSalesReportAsync(userId);

            return Ok(report);
        }
        [Authorize(Roles = "Seller")]
        [HttpGet("sales-summary")]
        public async Task<IActionResult>
    GetSalesSummary()
        {
            var userId =
                int.Parse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier
                    )!.Value);

            var summary =
                await _sellerService
                .GetSalesSummaryAsync(userId);

            return Ok(summary);
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<IActionResult> Create(SellerCreateDTO dto)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var seller = await _sellerService.CreateSellerAsync(userId, dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = seller.SellerId },
                seller);
        }
        [Authorize(Roles = "Seller")]
        [HttpPut("profile")]
        public async Task<IActionResult> Update(SellerUpdateDTO dto)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

            var updated =
                await _sellerService.UpdateSellerByUserIdAsync(
                    userId,
                    dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _sellerService.DeleteSellerAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
