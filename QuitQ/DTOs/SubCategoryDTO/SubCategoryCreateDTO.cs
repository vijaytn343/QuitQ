using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.SubCategoryDTO
{
    public class SubCategoryCreateDTO
    {
        [Range(1, int.MaxValue,ErrorMessage = "Please select a valid category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Subcategory name is required")]
        [StringLength(50,ErrorMessage = "Subcategory name cannot exceed 50 characters")]
        public string SubCategoryName { get; set; } = string.Empty;

        [StringLength(200,ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
    }
}
