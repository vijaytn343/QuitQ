namespace QuitQ.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        public int SellerId { get; set; }

        public int SubCategoryId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Brand { get; set; }

        public string? ImageUrl { get; set; }
        // Add this ✅
        public int QuantityAvailable { get; set; }
    }
}
