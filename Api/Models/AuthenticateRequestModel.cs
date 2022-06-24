using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class AuthenticateRequestModel
    {
        [Required, MaxLength(35)]
        public string UserName { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
