using Api.Models;
using Api.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService authentication;

        public AuthenticationController(
            IAuthenticationService authentication)
        {
            this.authentication = authentication;
        }

        [HttpPost, Route("api/1.0/authenticate")]
        [ProducesResponseType(200, Type = typeof(AuthenticationTokenModel))]
        public async Task<IActionResult> Register([FromBody] AuthenticateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await this.authentication.VerifyCredentials(model);

            if (token == null)
            {
                this.ModelState.AddModelError(nameof(model.UserName), "Invalid user name or password.");
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(token);
        }
    }
}
