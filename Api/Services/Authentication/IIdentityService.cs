using Api.Models;

namespace Api.Services.Authentication
{
    public interface IIdentityService
    {
        IdentityModel? GetIdentity();
    }
}