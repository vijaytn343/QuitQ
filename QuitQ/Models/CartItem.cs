namespace QuitQ.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // Navigation Properties

        // Many CartItems -> One Cart
        public Cart? Cart { get; set; }

        // Many CartItems -> One Product
        public Product? Product { get; set; }
    }
}
