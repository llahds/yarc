using Api.Models;
using Api.Services.Authentication;
using Api.Services.BackgroundJobs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService authentication;
        private readonly IBackgroundJobClient backgroundJob;

        public AuthenticationController(
            IAuthenticationService authentication,
            IBackgroundJobClient backgroundJob)
        {
            this.authentication = authentication;
            this.backgroundJob = backgroundJob;
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

            this.backgroundJob.Enqueue<BuildRecommendedPostQuery>(item => item.Build(model.UserName));

            return this.Ok(token);
        }
    }
}
