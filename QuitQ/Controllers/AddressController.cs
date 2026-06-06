using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.AddressDTOs;
using QuitQ.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QuitQ.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var address = await _addressService
                .GetAddressByIdAsync(id, userId);

            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(AddressCreateDTO dto)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                    .Value);

            var address = await _addressService
                .CreateAddressAsync(userId, dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = address.AddressId },
                address);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,AddressUpdateDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var updated = await _addressService
                .UpdateAddressAsync(userId,id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var deleted = await _addressService
                .DeleteAddressAsync(userId,id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
