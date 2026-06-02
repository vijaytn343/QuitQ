namespace QuitQ.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public int AddressId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
