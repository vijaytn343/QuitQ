using System.ComponentModel.DataAnnotations;

namespace QuitQ.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string? Phone { get; set; }

        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be Male, Female or Other")]
        public string? Gender { get; set; }
    }
}
