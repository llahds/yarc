using Api.Models;

namespace Api.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> CheckPassword(string userName, string password);
        Task<AuthenticationTokenModel> VerifyCredentials(AuthenticateRequestModel model);
    }
}