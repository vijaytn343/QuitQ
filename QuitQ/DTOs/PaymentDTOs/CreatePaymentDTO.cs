namespace QuitQ.DTOs.PaymentDTOs
{
    public class CreatePaymentDTO
    {
        public int OrderId { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}
