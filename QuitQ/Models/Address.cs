namespace QuitQ.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public int UserId { get; set; }

        public string FullAddress { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Pincode { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

      
        public User? User { get; set; }

       
        public ICollection<Order>? Orders { get; set; }
    }
}
