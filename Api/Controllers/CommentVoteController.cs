using Api.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class CommentVoteController : Controller
    {
        private readonly ICommentService comments;

        public CommentVoteController(
            ICommentService comments)
        {
            this.comments = comments;
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}/up")]
        public async Task<IActionResult> Up(int forumId, int postId, int commentId)
        {
            await this.comments.VoteUp(forumId, postId, commentId);

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}/down")]
        public async Task<IActionResult> Down(int forumId, int postId, int commentId)
        {
            await this.comments.VoteDown(forumId, postId, commentId);

            return this.Ok();
        }
    }
}
