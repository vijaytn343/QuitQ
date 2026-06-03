using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.CartDTOs
{
    public class AddToCartDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product")]
        public int ProductId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }
    }
}
