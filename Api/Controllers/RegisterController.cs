using Api.Models;
using Api.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService users;

        public RegisterController(
            IUserService users)
        {
            this.users = users;
        }

        [HttpPost, Route("api/1.0/register")]
        [ProducesResponseType(200, Type = typeof(AuthenticationTokenModel))]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (await users.EmailAlreadyExists(model.Email, null))
            {
                this.ModelState.AddModelError(nameof(model.Email), "Email already exists.");
            }

            if (await this.users.UserNameAlreadyExists(model.UserName, null))
            {
                this.ModelState.AddModelError(nameof(model.UserName), "User name already exists.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return this.Ok(await this.users.Register(model));
        }
    }
}
