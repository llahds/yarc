using Api.Models;
using Api.Services.Moderation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Moderation
{
    [Authorize]
    public class ModerationUserManagementController : ModerationController
    {
        public ModerationUserManagementController(
            IModerationService moderation)
            : base(moderation)
        {
        }

        [HttpGet, Route("api/1.0/moderation/forums/{forumId}/users")]
        [ProducesResponseType(200, Type = typeof(UserInfoModel[]))]
        public async Task<IActionResult> List(int forumId, int startAt, string query, int status)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                return this.Ok(await this.moderation.SearchForumMembers(forumId, startAt, query, status));
            });
        }

        [HttpGet, Route("api/1.0/moderation/users")]
        [ProducesResponseType(200, Type = typeof(UserInfoModel[]))]
        public async Task<IActionResult> Search(string query)
        {
            return this.Ok(await this.moderation.SearchUsers(query));
        }

        [HttpDelete, Route("api/1.0/moderation/forums/{forumId}/users/{userId}")]
        public async Task<IActionResult> Remove(int forumId, int userId)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                await this.moderation.RemoveForumMember(forumId, userId);

                return this.Ok();
            });
        }

        [HttpPost, Route("api/1.0/moderation/forums/{forumId}/users/{userId}/status")]
        public async Task<IActionResult> Approve(int forumId, int userId, [FromBody] ForumMemberStatusModel model)
        {
            return await this.VerifyCredentials(forumId, async () =>
            {
                await this.moderation.ChangeForumMemberStatus(forumId, userId, model);

                return this.Ok();
            });
        }
    }
}
