using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers.Moderation
{
    [Authorize]
    public class ModerationQueueController : Controller
    {
        private readonly YARCContext context;

        public ModerationQueueController(
            YARCContext context)
        {
            this.context = context;
        }

        [HttpGet, Route("api/1.0/moderation/{forumId}/queue")]
        [ProducesResponseType(200, Type = typeof(ReportQueueItemModel[]))]
        public async Task<IActionResult> List(int forumId, int startAt, int[] reasonIds)
        {
            var posts = await this.context
                .Posts
                .Where(P => 
                    P.ForumId == forumId 
                    && P.ReportedPosts.Any(R => reasonIds.Length == 0 || (reasonIds.Length > 0 && reasonIds.Contains(R.ReasonId))) 
                    && P.IsDeleted == false)
                .Select(P => new ReportQueueItemModel
                {
                    Post = new ForumPostListItemModel
                    {
                        Id = P.Id,
                        CreatedOn = P.CreatedOn,
                        Title = P.Title,
                        Forum = new KeyValueModel
                        {
                            Id = P.ForumId,
                            Name = P.Forum.Name
                        },
                        PostedBy = new PostedByModel
                        {
                            Id = P.PostedById,
                            Name = P.PostedBy.DisplayName ?? "[deleted]",
                            AvatarId = -1
                        }
                    },
                    Reasons = P
                        .ReportedPosts
                        .GroupBy(R => R.Reason.Label)
                        .Select(R => R.Key)
                        .ToArray()
                })
                .Skip(startAt)
                .Take(25)
                .ToArrayAsync();

            return this.Ok(posts);
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/{postId}/approve")]
        public async Task<IActionResult> Approve(int forumId, int postId)
        {
            var post = await this.context
                .Posts
                .Include(P => P.ReportedPosts)
                .Where(U => 
                    U.ForumId == forumId 
                    && U.Id == postId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (post != null)
            {
                post.IsHidden = false;

                context.RemoveRange(post.ReportedPosts);

                await this.context.SaveChangesAsync();
            }

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/{postId}/reject")]
        public async Task<IActionResult> Reject(int forumId, int postId)
        {
            var post = await this.context
                .Posts
                .Where(U => 
                    U.ForumId == forumId 
                    && U.Id == postId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (post != null)
            {
                post.IsDeleted = true;

                await this.context.SaveChangesAsync();
            }

            return this.Ok();
        }
    }
}
