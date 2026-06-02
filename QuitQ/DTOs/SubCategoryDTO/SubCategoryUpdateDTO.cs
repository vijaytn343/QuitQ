namespace QuitQ.DTOs.SubCategoryDTO
{
    public class SubCategoryUpdateDTO
    {
        public int CategoryId { get; set; }

        public string SubCategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
