namespace QuitQ.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal PriceAtPurchase { get; set; }

        // Navigation Properties

        // Many OrderItems -> One Order
        public Order? Order { get; set; }

        // Many OrderItems -> One Product
        public Product? Product { get; set; }
    }
}
