using Api.Models;
using Api.Services.Moderation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Moderation
{
    [Authorize]
    public class ModerationQueueController : ModerationController
    {
        public ModerationQueueController(
            IModerationService moderation)
            : base(moderation)
        {

        }

        [HttpGet, Route("api/1.0/moderation/{forumId}/queue")]
        [ProducesResponseType(200, Type = typeof(ReportQueueItemModel[]))]
        public async Task<IActionResult> List(int forumId, int startAt, int[] reasonIds)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                return this.Ok(await this.moderation.GetModerationQueue(forumId, startAt, reasonIds));
            });
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/posts/{postId}/approve")]
        public async Task<IActionResult> ApprovePost(int forumId, int postId)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                await this.moderation.ApprovePost(forumId, postId);

                return this.Ok();
            });
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/posts/{postId}/reject")]
        public async Task<IActionResult> RejectPost(int forumId, int postId)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                await this.moderation.RejectPost(forumId, postId);

                return this.Ok();
            });
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/comments/{commentId}/approve")]
        public async Task<IActionResult> ApproveComment(int forumId, int commentId)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                await this.moderation.ApproveComment(forumId, commentId);

                return this.Ok();
            });
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/comments/{commentId}/reject")]
        public async Task<IActionResult> RejectComment(int forumId, int commentId)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                await this.moderation.RejectComment(forumId, commentId);

                return this.Ok();
            });
        }
    }
}
