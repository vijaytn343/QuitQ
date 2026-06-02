namespace QuitQ.DTOs.SubCategoryDTO
{
    public class SubCategoryResponseDTO
    {

        public int SubCategoryId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string SubCategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
