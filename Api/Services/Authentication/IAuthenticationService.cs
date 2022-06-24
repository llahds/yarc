using Api.Models;

namespace Api.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationTokenModel> VerifyCredentials(AuthenticateRequestModel model);
    }
}