namespace QuitQ.Models
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }

        public int CategoryId { get; set; }

        public string SubCategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation Properties

        // Many SubCategories -> One Category
        public Category? Category { get; set; }

        // One SubCategory -> Many Products
        public ICollection<Product>? Products { get; set; }
    }
}
