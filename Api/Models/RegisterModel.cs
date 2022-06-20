using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class RegisterModel
    {
        [Required, MaxLength(125), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(35), RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The display name can only contain letters and numbers.")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
