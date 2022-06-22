using Api.Data;
using Api.Data.Entities;
using Api.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Authorize]
    public class CommentVoteController : Controller
    {
        private readonly YARCContext context;
        private readonly IIdentityService identity;

        public CommentVoteController(
            YARCContext context,
            IIdentityService identity)
        {
            this.context = context;
            this.identity = identity;
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}/up")]
        public async Task<IActionResult> Up(int forumId, int postId, int commentId)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context
                .CommentVotes
                .Where(P => P.CommentId == commentId && P.Comment.PostId == postId && P.Comment.Post.ForumId == forumId && P.ById == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new CommentVote
                {
                    CreatedOn = DateTime.UtcNow,
                    CommentId = commentId,
                    ById = this.identity.GetIdentity().Id
                };

                await this.context.AddAsync(entity);
            }

            entity.Vote = 1;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}/down")]
        public async Task<IActionResult> Down(int forumId, int postId, int commentId)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context
                .CommentVotes
                .Where(P => P.CommentId == commentId && P.Comment.PostId == postId && P.Comment.Post.ForumId == forumId && P.ById == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new CommentVote
                {
                    CreatedOn = DateTime.UtcNow,
                    CommentId = commentId,
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
