using Api.Data;
using Api.Models;
using Api.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly YARCContext context;
        private readonly ITokenGeneratorService tokens;

        public AuthenticationController(
            YARCContext context,
            ITokenGeneratorService tokens)
        {
            this.context = context;
            this.tokens = tokens;
        }

        [HttpPost, Route("api/1.0/authenticate")]
        [ProducesResponseType(200, Type = typeof(AuthenticationTokenModel))]
        public async Task<IActionResult> Register([FromBody] AuthenticateRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await this.context.Users
                .Where(U => U.UserName == model.UserName)
                .FirstOrDefaultAsync();

            if (entity?.Password != model.Password)
            {
                this.ModelState.AddModelError(nameof(model.UserName), "Invalid user name or password.");
                return this.BadRequest(this.ModelState);
            }

            var upn = new Claim(ClaimTypes.NameIdentifier, entity.UserName);
            var token = await this.tokens.Generate(new[] { upn });

            return this.Ok(new AuthenticationTokenModel
            {
                UserName = model.UserName,
                Token = token
            });
        }
    }
}
