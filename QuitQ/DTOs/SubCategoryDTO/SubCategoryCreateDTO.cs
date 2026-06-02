namespace QuitQ.DTOs.SubCategoryDTO
{
    public class SubCategoryCreateDTO
    {
        public int CategoryId { get; set; }

        public string SubCategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
