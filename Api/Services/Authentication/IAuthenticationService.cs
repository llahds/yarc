using Api.Models;

namespace Api.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> CheckHashedPassword(string userName, string password);
        Task<bool> CheckPlainTextPassword(string userName, string password);
        Task<AuthenticationTokenModel> VerifyCredentials(AuthenticateRequestModel model);
    }
}