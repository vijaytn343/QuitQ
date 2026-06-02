using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.SellerDTOs;
using QuitQ.Services.Interfaces;

namespace QuitQ.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Create(SellerCreateDTO dto)
        {
            var seller = await _sellerService.CreateSellerAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = seller.SellerId },
                seller);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SellerUpdateDTO dto)
        {
            var updated = await _sellerService.UpdateSellerAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

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
