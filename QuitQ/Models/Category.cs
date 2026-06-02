namespace QuitQ.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navigation Property

        // One Category -> Many SubCategories
        public ICollection<SubCategory>? SubCategories { get; set; }
    }

}
