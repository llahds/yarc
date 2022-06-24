using Api.Models;
using Api.Services.Moderation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Moderation
{
    [Authorize]
    public class ForumPostSettingsController : ModerationController
    {
        public ForumPostSettingsController(
            IModerationService moderation)
            : base(moderation)
        {
            
        }

        [HttpGet, Route("api/1.0/moderation/forums/{forumId}/posts/settings")]
        [ProducesResponseType(200, Type = typeof(ForumPostSettingsModel))]
        public async Task<IActionResult> Get(int forumId)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                return this.Ok(await this.moderation.GetForumPostSettings(forumId));
            });
        }

        [HttpPut, Route("api/1.0/moderation/forums/{forumId}/posts/settings")]
        public async Task<IActionResult> Update(int forumId, [FromBody] ForumPostSettingsModel model)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                await this.moderation.UpdateForumPostSettings(forumId, model);

                return this.Ok();
            });
        }
    }
}
