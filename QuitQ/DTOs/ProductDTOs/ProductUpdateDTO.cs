namespace QuitQ.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {
        public int SubCategoryId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Brand { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
