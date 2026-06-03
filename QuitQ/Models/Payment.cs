namespace QuitQ.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string? TransactionId { get; set; }

      

        // Many Payments -> One Order
        public Order? Order { get; set; }
    }
}
