using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.ProductDTOs
{
    public class ProductFilterDTO
    {
        public string? Keyword { get; set; }

        public int? CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        public string? Brand { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}
