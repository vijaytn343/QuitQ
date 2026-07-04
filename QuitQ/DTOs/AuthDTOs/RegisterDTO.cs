using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.AuthDTOs
{
    public class RegisterDTO
    {


        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$",
    ErrorMessage = "Phone number must be exactly 10 digits")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(Customer|Seller)$", ErrorMessage = "Role must be Customer or Seller")]
        public string Role { get; set; } = "Customer";
    }
}
