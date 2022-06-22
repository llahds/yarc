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
            var items = await this.context
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
                .Take(25)
                .ToListAsync();

            var comments = await this.context
                .Comments
                .Where(C =>
                    C.Post.ForumId == forumId
                    && C.ReportedComments.Any(R => reasonIds.Length == 0 || (reasonIds.Length > 0 && reasonIds.Contains(R.ReasonId)))
                    && C.IsDeleted == false
                )
                .Select(C => new ReportQueueItemModel
                {
                    Comment = new CommentInfoModel
                    {
                        Id = C.Id,
                        CreatedOn = C.CreatedOn,
                        Text = C.Text,
                        PostedBy = new PostedByModel
                        {
                            Id = C.PostedById,
                            Name = C.PostedBy.UserName
                        }
                    },
                    Reasons = C
                        .ReportedComments
                        .GroupBy(R => R.Reason.Label)
                        .Select(R => R.Key)
                        .ToArray()
                })
                .Take(25)
                .ToListAsync();

            items.AddRange(comments);

            return this.Ok(items);
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/posts/{postId}/approve")]
        public async Task<IActionResult> ApprovePost(int forumId, int postId)
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

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/posts/{postId}/reject")]
        public async Task<IActionResult> RejectPost(int forumId, int postId)
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

                context.RemoveRange(post.ReportedPosts);

                await this.context.SaveChangesAsync();
            }

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/comments/{commentId}/approve")]
        public async Task<IActionResult> ApproveComment(int forumId, int commentId)
        {
            var comment = await this.context
                .Comments
                .Include(P => P.ReportedComments)
                .Where(U =>
                    U.Post.ForumId == forumId
                    && U.Id == commentId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (comment != null)
            {
                comment.IsHidden = false;

                context.RemoveRange(comment.ReportedComments);

                await this.context.SaveChangesAsync();
            }

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/moderation/{forumId}/queue/comments/{commentId}/reject")]
        public async Task<IActionResult> RejectComment(int forumId, int commentId)
        {
            var comment = await this.context
                .Comments
                .Include(P => P.ReportedComments)
                .Where(U =>
                    U.Post.ForumId == forumId
                    && U.Id == commentId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (comment != null)
            {
                comment.IsDeleted = true;

                context.RemoveRange(comment.ReportedComments);

                await this.context.SaveChangesAsync();
            }

            return this.Ok();
        }
    }
}
