using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers.Moderation
{
    [Authorize]
    public class ModerationUserManagementController : Controller
    {
        private readonly YARCContext context;

        public ModerationUserManagementController(
            YARCContext context)
        {
            this.context = context;
        }

        [HttpGet, Route("api/1.0/moderation/forums/{forumId}/users")]
        [ProducesResponseType(200, Type = typeof(UserInfoModel[]))]
        public async Task<IActionResult> List(int forumId, int startAt, string query, int status)
        {
            query = query ?? "";

            var model = await this.context
                .Users
                .Where(U => 
                    U.Forums.Any(F => F.ForumId == forumId && F.Status == status)
                    && (query.Length == 0 || query.Length > 0 && U.UserName.StartsWith(query))
                    && U.IsDeleted == false)
                .Select(U => new UserInfoModel
                {
                    Id = U.Id,
                    UserName = U.UserName
                })
                .Take(25)
                .Skip(startAt)
                .ToArrayAsync();

            return this.Ok(model);
        }

        [HttpGet, Route("api/1.0/moderation/users")]
        [ProducesResponseType(200, Type = typeof(UserInfoModel[]))]
        public async Task<IActionResult> Search(string query)
        {
            query = query ?? "";

            var model = await this.context
                .Users
                .Where(U => 
                    (query.Length == 0 || query.Length > 0 && U.UserName.StartsWith(query))
                    && U.IsDeleted == false
                )
                .Select(U => new UserInfoModel
                {
                    Id = U.Id,
                    UserName = U.UserName
                })
                .Take(25)
                .OrderBy(U => U.UserName)
                .ToArrayAsync();

            return this.Ok(model);
        }

        [HttpDelete, Route("api/1.0/moderation/forums/{forumId}/users/{userId}")]
        public async Task<IActionResult> Remove(int forumId, int userId)
        {
            var entity = await this.context
                .ForumMembers
                .Where(U => U.ForumId == forumId && U.MemberId == userId)
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                this.context.Remove(entity);

                await this.context.SaveChangesAsync();
            }

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/moderation/forums/{forumId}/users/{userId}/status")]
        public async Task<IActionResult> Approve(int forumId, int userId, [FromBody] ForumMemberStatusModel model)
        {
            var entity = await this.context
                .ForumMembers
                .Where(U => U.ForumId == forumId && U.Id == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new Data.Entities.ForumMember
                {
                    ForumId = forumId,
                    MemberId = userId
                };

                await this.context.AddAsync(entity);
            }

            entity.CreatedOn = DateTime.UtcNow;
            entity.Status = model.Status;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
