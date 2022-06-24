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
            if (await this.CheckPassword(model.UserName, model.Password) == false)
            {
                return null;
            }

            var entity = await this.context.Users
                .Where(U => U.UserName == model.UserName)
                .FirstOrDefaultAsync();

            var upn = new Claim(ClaimTypes.NameIdentifier, entity.UserName);
            var id = new Claim("Id", entity.Id.ToString());
            var token = await this.tokens.Generate(new[] { upn, id });

            return new AuthenticationTokenModel
            {
                UserName = model.UserName,
                Token = token
            };
        }

        public async Task<bool> CheckPassword(string userName, string password)
        {
            var entity = await this.context.Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            if (entity?.Password != password)
            {
                return false;
            }

            return true;
        }
    }
}
