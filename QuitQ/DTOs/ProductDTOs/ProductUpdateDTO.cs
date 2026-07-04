using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {
        [Range(1, int.MaxValue,ErrorMessage = "Please select a valid subcategory")]
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(500,ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Range(0.01, 1000000,ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [StringLength(50,ErrorMessage = "Brand cannot exceed 50 characters")]
        public string? Brand { get; set; }

       
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        [Range(0, int.MaxValue,ErrorMessage = "Quantity cannot be negative")]
        public int QuantityAvailable { get; set; }
    }
}
