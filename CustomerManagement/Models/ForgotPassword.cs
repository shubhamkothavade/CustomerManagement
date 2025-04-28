using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Models
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}
