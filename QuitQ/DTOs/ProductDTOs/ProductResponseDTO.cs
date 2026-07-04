namespace QuitQ.DTOs.ProductDTOs
{
    public class ProductResponseDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Brand { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        public int QuantityAvailable { get; set; }

        public string? CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }

        public string? SellerName { get; set; }
    }
}
