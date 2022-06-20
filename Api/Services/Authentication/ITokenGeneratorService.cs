using Api.Models;
using System.Security.Claims;

namespace Api.Services.Authentication
{
    public interface ITokenGeneratorService
    {
        Task<string> Generate(IEnumerable<Claim> claims);
    }
}