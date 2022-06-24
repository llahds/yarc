using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class ChangeEmailModel
    {
        [Required, MaxLength(125), EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";

    }
}
