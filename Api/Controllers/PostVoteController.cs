using Api.Data;
using Api.Data.Entities;
using Api.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]
    public class PostVoteController : Controller
    {
        private readonly YARCContext context;
        private readonly IIdentityService identity;

        public PostVoteController(
            YARCContext context,
            IIdentityService identity)
        {
            this.context = context;
            this.identity = identity;
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/up")]
        public async Task<IActionResult> Up(int forumId, int postId)
        {
            var entity = await this.context
                .PostVotes
                .Where(P => P.PostId == postId && P.Post.ForumId == forumId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new PostVote
                {
                    CreatedOn = DateTime.UtcNow,
                    PostId = postId,
                    ById = this.identity.GetIdentity().Id
                };

                await this.context.AddAsync(entity);
            }

            entity.Vote = 1;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/down")]
        public async Task<IActionResult> Down(int forumId, int postId)
        {
            var entity = await this.context
                .PostVotes
                .Where(P => P.PostId == postId && P.Post.ForumId == forumId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new PostVote
                {
                    CreatedOn = DateTime.UtcNow,
                    PostId = postId,
                    ById = this.identity.GetIdentity().Id
                };

                await this.context.AddAsync(entity);
            }

            entity.Vote = -1;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
