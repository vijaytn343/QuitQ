namespace QuitQ.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }

        public string OrderStatus { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public decimal TotalAmount { get; set; }

      

        // Many Orders -> One User
        public User? User { get; set; }

        // Many Orders -> One Address
        public Address? Address { get; set; }

        // One Order -> Many OrderItems
        public ICollection<OrderItem>? OrderItems { get; set; }

        // One Order -> Many Payments
        public ICollection<Payment>? Payments { get; set; }
    }
}
