using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly YARCContext context;
        private readonly ITokenGeneratorService tokens;

        public AuthenticationService(
            YARCContext context,
            ITokenGeneratorService tokens)
        {
            this.context = context;
            this.tokens = tokens;
        }

        public async Task<AuthenticationTokenModel> VerifyCredentials(AuthenticateRequestModel model)
        {
            var entity = await this.context.Users
                .Where(U => U.UserName == model.UserName)
                .FirstOrDefaultAsync();

            if (entity?.Password != model.Password)
            {
                return null;
            }

            var upn = new Claim(ClaimTypes.NameIdentifier, entity.UserName);
            var id = new Claim("Id", entity.Id.ToString());
            var token = await this.tokens.Generate(new[] { upn, id });

            return new AuthenticationTokenModel
            {
                UserName = model.UserName,
                Token = token
            };
        }
    }
}
