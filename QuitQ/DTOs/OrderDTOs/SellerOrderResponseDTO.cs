namespace QuitQ.DTOs.OrderDTOs
{
    public class SellerOrderResponseDTO
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal PriceAtPurchase { get; set; }

        public string OrderStatus { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
    }
}
