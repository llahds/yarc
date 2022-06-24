using Api.Services.Moderation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Moderation
{
    public class ModerationController : Controller
    {
        protected readonly IModerationService moderation;

        public ModerationController(
            IModerationService moderation)
        {
            this.moderation = moderation;
        }

        public async Task<IActionResult> VerifyCredentials(int forumId, Func<Task<IActionResult>> func)
        {
            var isModerator = await this.moderation.IsModerator(forumId);

            var isOwner = await this.moderation.IsOwner(forumId);

            if (isOwner || isModerator)
            {
                return await func();
            }

            return this.Unauthorized();
        }
    }
}
