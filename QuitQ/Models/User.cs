namespace QuitQ.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public int RoleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

      

        // Many Users -> One Role
        public Role? Role { get; set; }

        // One User -> One Seller
        public Seller? Seller { get; set; }

        // One User -> Many Addresses
        public ICollection<Address>? Addresses { get; set; }

        // One User -> One Cart
        public Cart? Cart { get; set; }

        // One User -> Many Orders
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        // One User -> Many Password Reset Tokens
    

    }
}
