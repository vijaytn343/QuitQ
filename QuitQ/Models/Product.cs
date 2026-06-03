namespace QuitQ.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public int SellerId { get; set; }

        public int SubCategoryId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Brand { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

      

        // Many Products -> One Seller
        public Seller? Seller { get; set; }

        // Many Products -> One SubCategory
        public SubCategory? SubCategory { get; set; }

        // One Product -> One Inventory
        public Inventory? Inventory { get; set; }

        // One Product -> Many CartItems
        public ICollection<CartItem>? CartItems { get; set; }

        // One Product -> Many OrderItems
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
