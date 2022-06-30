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
        private readonly IPasswordHashService passwords;

        public AuthenticationService(
            YARCContext context,
            ITokenGeneratorService tokens,
            IPasswordHashService passwords)
        {
            this.context = context;
            this.tokens = tokens;
            this.passwords = passwords;
        }

        public async Task<AuthenticationTokenModel> VerifyCredentials(AuthenticateRequestModel model)
        {
            var changePassword = false;

            if (await this.CheckHashedPassword(model.UserName, model.Password) == false)
            {
                if (await this.CheckPlainTextPassword(model.UserName, model.Password) == true)
                {
                    changePassword = true;
                }
                else
                {
                    return null;
                }
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
                Token = token,
                ChangePassword = changePassword
            };
        }

        public async Task<bool> CheckHashedPassword(string userName, string password)
        {
            var entity = await this.context.Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            try
            {
                return await this.passwords.Validate(password, entity?.Password);
            }
            catch (Exception e)
            {
                // TODO: add logging
                return false;
            }
        }

        public async Task<bool> CheckPlainTextPassword(string userName, string password)
        {
            var entity = await this.context.Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            return entity?.Password == password;
        }
    }
}
