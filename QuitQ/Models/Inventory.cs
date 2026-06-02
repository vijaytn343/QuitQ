namespace QuitQ.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public int QuantityAvailable { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation Property

        // One Inventory -> One Product
        public Product? Product { get; set; }
    }
}
