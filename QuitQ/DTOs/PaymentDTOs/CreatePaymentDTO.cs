using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.PaymentDTOs
{
    public class CreatePaymentDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid order")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(20, ErrorMessage = "Payment method cannot exceed 20 characters")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Range(0.01, 1000000, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
    }
}
