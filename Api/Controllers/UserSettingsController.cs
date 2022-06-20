using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]
    public class UserSettingsController : Controller
    {
        private readonly IMapper mapper;
        private readonly YARCContext context;
        private readonly IIdentityService identity;

        public UserSettingsController(
            IMapper mapper,
            YARCContext context,
            IIdentityService identity)
        {
            this.mapper = mapper;
            this.context = context;
            this.identity = identity;
        }

        [HttpGet, Route("api/1.0/user-settings")]
        [ProducesResponseType(200, Type = typeof(UserSettingsModel))]
        public async Task<IActionResult> Get()
        {
            var userName = this.identity.GetIdentity().UserName;

            var entity = await this.context
                .Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            var model = this.mapper.Map<UserSettingsModel>(entity);

            return this.Ok(model);
        }

        [HttpPut, Route("api/1.0/user-settings")]
        public async Task<IActionResult> Update([FromBody] UserSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = this.identity.GetIdentity().UserName;

            var entity = await this.context
                .Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            entity.DisplayName = model.DisplayName;
            entity.About = model.About;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPut, Route("api/1.0/user-settings/user-name")]
        public async Task<IActionResult> UpdateUserName([FromBody] ChangeUserNameModel model)
        {
            var userName = this.identity.GetIdentity().UserName;

            var userNameAlreadyExists = await context
                .Users
                .AnyAsync(U => U.UserName == model.UserName
                    && U.UserName != userName);

            if (userNameAlreadyExists)
            {
                this.ModelState.AddModelError(nameof(model.UserName), "User name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await this.context
                .Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            if (entity?.Password != model.Password)
            {
                this.ModelState.AddModelError(nameof(model.Password), "Invalid password.");
                return this.BadRequest(this.ModelState);
            }

            entity.UserName = model.UserName;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPut, Route("api/1.0/user-settings/email")]
        public async Task<IActionResult> UpdateEmail([FromBody] ChangeEmailModel model)
        {
            var userName = this.identity.GetIdentity().UserName;

            var emailAlreadyExists = await context
                .Users
                .AnyAsync(U => U.Email == model.Email
                    && U.UserName != userName);

            if (emailAlreadyExists)
            {
                this.ModelState.AddModelError(nameof(model.Email), "User name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await this.context
                .Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            if (entity?.Password != model.Password)
            {
                this.ModelState.AddModelError(nameof(model.Password), "Invalid password.");
                return this.BadRequest(this.ModelState);
            }

            entity.Email = model.Email;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPut, Route("api/1.0/user-settings/password")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordModel model)
        {
            var userName = this.identity.GetIdentity().UserName;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await this.context
                .Users
                .Where(U => U.UserName == userName)
                .FirstOrDefaultAsync();

            if (entity?.Password != model.OldPassword)
            {
                this.ModelState.AddModelError(nameof(model.OldPassword), "Invalid password.");
                return this.BadRequest(this.ModelState);
            }

            entity.Password = model.Password;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
