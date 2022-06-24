using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly ITokenGeneratorService tokens;
        private readonly IIdentityService identity;
        
        public UserService(
            YARCContext context,
            IMapper mapper,
            ITokenGeneratorService tokens,
            IIdentityService identity)
        {
            this.context = context;
            this.mapper = mapper;
            this.tokens = tokens;
            this.identity = identity;
        }

        public async Task<bool> EmailAlreadyExists(string email, int? userId)
        {
            return await context
                .Users
                .AnyAsync(U => U.Email == email && (userId == null || U.Id != userId));
        }

        public async Task<bool> UserNameAlreadyExists(string userName, int? userId)
        {
            return await context
                .Users
                .AnyAsync(U => U.UserName == userName && (userId == null || U.Id != userId));
        }

        public async Task<AuthenticationTokenModel> Register(RegisterModel model)
        {
            var entity = this.mapper.Map<User>(model);

            await this.context.Users.AddAsync(entity);

            await this.context.SaveChangesAsync();

            var upn = new Claim(ClaimTypes.NameIdentifier, entity.UserName);
            var id = new Claim("Id", entity.Id.ToString());
            var token = await this.tokens.Generate(new[] { upn, id });

            return new AuthenticationTokenModel
            {
                UserName = model.UserName,
                Token = token
            };
        }

        public async Task<UserSettingsModel> GetUserSettings()
        {
            var userName = this.identity.GetIdentity().UserName;

            var entity = await this.context
                .Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            return this.mapper.Map<UserSettingsModel>(entity);
        }

        public async Task UpdateUserSettings(UserSettingsModel model)
        {
            var userName = this.identity.GetIdentity().UserName;

            var entity = await this.context
                .Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            entity.DisplayName = model.DisplayName;
            entity.About = model.About;

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateUserName(string userName)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context.Users.FirstOrDefaultAsync(U => U.Id == userId);

            entity.UserName = userName;

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateEmail(string email)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context.Users.FirstOrDefaultAsync(U => U.Id == userId);

            entity.Email = email;

            await this.context.SaveChangesAsync();
        }

        public async Task UpdatePassword(string password)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context.Users.FirstOrDefaultAsync(U => U.Id == userId);

            entity.Password = password;

            await this.context.SaveChangesAsync();
        }
    }
}
