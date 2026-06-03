using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.AddressDTOs;
using QuitQ.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace QuitQ.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);

            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Create(
            int userId,
            AddressCreateDTO dto)
        {
            var address = await _addressService
                .CreateAddressAsync(userId, dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = address.AddressId },
                address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            AddressUpdateDTO dto)
        {
            var updated = await _addressService
                .UpdateAddressAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _addressService
                .DeleteAddressAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
