using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using Api.Services.Users;
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
        private readonly IUserService users;
        private readonly IAuthenticationService authentication;

        public UserSettingsController(
            IMapper mapper,
            YARCContext context,
            IIdentityService identity,
            IUserService users,
            IAuthenticationService authentication)
        {
            this.mapper = mapper;
            this.context = context;
            this.identity = identity;
            this.users = users;
            this.authentication = authentication;
        }

        [HttpGet, Route("api/1.0/user-settings")]
        [ProducesResponseType(200, Type = typeof(UserSettingsModel))]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.users.GetUserSettings());
        }

        [HttpPut, Route("api/1.0/user-settings")]
        public async Task<IActionResult> Update([FromBody] UserSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await this.users.UpdateUserSettings(model);

            return this.Ok();
        }

        [HttpPut, Route("api/1.0/user-settings/user-name")]
        public async Task<IActionResult> UpdateUserName([FromBody] ChangeUserNameModel model)
        {
            var userId = this.identity.GetIdentity().Id;

            if (await this.users.UserNameAlreadyExists(model.UserName, userId))
            {
                this.ModelState.AddModelError(nameof(model.UserName), "User name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = this.identity.GetIdentity().UserName;

            if (await this.authentication.CheckPassword(userName, model.Password) == false)
            {
                this.ModelState.AddModelError(nameof(model.Password), "Invalid password.");
                return this.BadRequest(this.ModelState);
            }

            await this.users.UpdateUserName(model.UserName);

            return this.Ok();
        }

        [HttpPut, Route("api/1.0/user-settings/email")]
        public async Task<IActionResult> UpdateEmail([FromBody] ChangeEmailModel model)
        {
            var userId = this.identity.GetIdentity().Id;

            if (await this.users.EmailAlreadyExists(model.Email, userId))
            {
                this.ModelState.AddModelError(nameof(model.Email), "Email already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userName = this.identity.GetIdentity().UserName;

            if (await this.authentication.CheckPassword(userName, model.Password))
            {
                this.ModelState.AddModelError(nameof(model.Password), "Invalid password.");
                return this.BadRequest(this.ModelState);
            }

            await this.users.UpdateEmail(model.Email);

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

            if (await this.authentication.CheckPassword(userName, model.OldPassword) == false)
            {
                this.ModelState.AddModelError(nameof(model.OldPassword), "Invalid password.");
                return this.BadRequest(this.ModelState);
            }

            await this.users.UpdatePassword(model.Password);

            return this.Ok();
        }
    }
}
