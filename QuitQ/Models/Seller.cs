namespace QuitQ.Models
{
    public class Seller
    {

        public int SellerId { get; set; }

        public int UserId { get; set; }

        public string StoreName { get; set; } = string.Empty;

        public string? GSTNumber { get; set; }

        public string? BusinessEmail { get; set; }

        public string? AccountHolderName { get; set; }

        public string? AccountNumber { get; set; }

        public string? IFSCCode { get; set; }

        public string? BankName { get; set; }

        // Navigation Properties

        // One Seller -> One User
        public User? User { get; set; }

        // One Seller -> Many Products
        public ICollection<Product>? Products { get; set; }
    }
}
