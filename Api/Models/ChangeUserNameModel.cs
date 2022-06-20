using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class ChangeUserNameModel
    {
        [Required, MaxLength(35), RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The display name can only contain letters and numbers.")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
