namespace QuitQ.DTOs.CartDTOs
{
    public class CartResponseDTO
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public List<CartItemResponseDTO> CartItems { get; set; } = new();

        public decimal TotalAmount { get; set; }
    }

    public class CartItemResponseDTO
    {
        public int CartItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }
    }
}
