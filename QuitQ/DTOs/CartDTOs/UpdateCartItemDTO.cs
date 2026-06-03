using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.CartDTOs
{
    public class UpdateCartItemDTO
    {
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }
    }
}
