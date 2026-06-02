namespace QuitQ.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties

        // One Cart -> One User
        public User? User { get; set; }

        // One Cart -> Many CartItems
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
