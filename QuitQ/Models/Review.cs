using System.ComponentModel.DataAnnotations;

namespace QuitQ.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(500)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.Now;

        public bool IsActive { get; set; }
            = true;

        public User? User { get; set; }

        public Product? Product { get; set; }
    }
}
