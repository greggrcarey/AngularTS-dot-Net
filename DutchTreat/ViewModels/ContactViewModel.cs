using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class ContactViewModel
    {

        [Required]
        [MinLength(5)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Subject { get; set; } = string.Empty;
        [Required]
        [MaxLength(250, ErrorMessage = "Too Long")]
        public string Message { get; set; } = string.Empty;
    }
}
