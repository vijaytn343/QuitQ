using Microsoft.AspNetCore.Mvc;
using QuitQ.DTOs.SubCategoryDTO;
using QuitQ.Services.Interfaces;

namespace QuitQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subCategories = await _subCategoryService.GetAllSubCategoriesAsync();
            return Ok(subCategories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);

            if (subCategory == null)
                return NotFound();

            return Ok(subCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubCategoryCreateDTO dto)
        {
            var subCategory = await _subCategoryService.CreateSubCategoryAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = subCategory.SubCategoryId },
                subCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SubCategoryUpdateDTO dto)
        {
            var updated = await _subCategoryService.UpdateSubCategoryAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _subCategoryService.DeleteSubCategoryAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
