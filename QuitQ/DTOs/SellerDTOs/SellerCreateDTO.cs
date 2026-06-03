using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.SellerDTOs
{
    public class SellerCreateDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid user")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Store name is required")]
        [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters")]
        public string StoreName { get; set; } = string.Empty;

        [StringLength(15, ErrorMessage = "GST number cannot exceed 15 characters")]
        public string? GSTNumber { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid business email")]
        public string? BusinessEmail { get; set; }

        [StringLength(100, ErrorMessage = "Account holder name cannot exceed 100 characters")]
        public string? AccountHolderName { get; set; }

        [StringLength(30, ErrorMessage = "Account number cannot exceed 30 characters")]
        public string? AccountNumber { get; set; }

        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Please enter a valid IFSC code")]
        public string? IFSCCode { get; set; }

        [StringLength(100, ErrorMessage = "Bank name cannot exceed 100 characters")]
        public string? BankName { get; set; }
    }
}
