using Api.Models;
using Api.Services.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly IPostService posts;
        private readonly IPostViewService views;

        public ForumPostController(
            IPostService posts,
            IPostViewService views)
        {
            this.posts = posts;
            this.views = views;
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts")]
        [ProducesResponseType(200, Type = typeof(ForumPostListItemModel[]))]
        public async Task<IActionResult> List(int forumId, int startAt)
        {
            return this.Ok(await this.posts.List(forumId, startAt));
        }

        [HttpGet, Route("api/1.0/forums/posts/popular")]
        [ProducesResponseType(200, Type = typeof(ForumPostListItemModel[]))]
        public async Task<IActionResult> Popular(int startAt)
        {
            return this.Ok(await this.views.Popular(startAt));
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        [ProducesResponseType(200, Type = typeof(ForumPostViewModel))]
        public async Task<IActionResult> Get(int forumId, int postId)
        {
            var model = await this.posts.Get(forumId, postId);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts")]
        [ProducesResponseType(200, Type = typeof(IdModel<int>))]
        public async Task<IActionResult> Create(int forumId, [FromBody] EditForumPostModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(await this.posts.Create(forumId, model));
        }

        [HttpPut, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        public async Task<IActionResult> Update(int forumId, int postId, [FromBody] EditForumPostModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            if (await this.posts.Update(forumId, postId, model) == false)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpDelete, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        public async Task<IActionResult> Remove(int forumId, int postId)
        {
            if (await this.posts.Remove(forumId, postId) == false)
            {
                return this.NotFound();
            }

            return this.Ok();
        }
    }
}
