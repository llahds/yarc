using Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services.Authentication
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration configuration;

        public TokenGeneratorService(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> Generate(IEnumerable<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims);

            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = configuration["authentication:secretKey"];
            var expirationInMinutes = int.Parse(configuration["authentication:expirationInMinutes"]);

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Task.FromResult(accessToken);
        }
    }
}
