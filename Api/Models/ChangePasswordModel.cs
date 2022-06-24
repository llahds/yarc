using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}
