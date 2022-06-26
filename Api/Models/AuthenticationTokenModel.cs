namespace Api.Models
{
    public class AuthenticationTokenModel
    {
        public string Token { get; set; } = "";
        public string UserName { get; set; } = "";
        public bool ChangePassword { get; set; }
    }
}
