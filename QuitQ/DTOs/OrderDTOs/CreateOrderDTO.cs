using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid address")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(20, ErrorMessage = "Payment method cannot exceed 20 characters")]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
