using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.CategoryDTOs
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50,ErrorMessage = "Category name cannot exceed 50 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200,ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
    }
}
