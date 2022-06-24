using Api.Services.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class PostVoteController : Controller
    {
        private readonly IPostService posts;

        public PostVoteController(
            IPostService posts)
        {
            this.posts = posts;
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/up")]
        public async Task<IActionResult> Up(int forumId, int postId)
        {
            await this.posts.VoteUp(forumId, postId);

            return this.Ok();
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/down")]
        public async Task<IActionResult> Down(int forumId, int postId)
        {
            await this.posts.VoteDown(forumId, postId);

            return this.Ok();
        }
    }
}
