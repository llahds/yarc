using Api.Data.Entities;
using Api.Models;
using Api.Services.Forums;
using Api.Services.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly IPostService posts;
        private readonly IPostViewService views;
        private readonly IForumService forums;

        public ForumPostController(
            IPostService posts,
            IPostViewService views,
            IForumService forums)
        {
            this.posts = posts;
            this.views = views;
            this.forums = forums;
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts")]
        [ProducesResponseType(200, Type = typeof(ForumPostListItemModel[]))]
        public async Task<IActionResult> List(int forumId, int startAt)
        {
            if (await this.forums.CanAccessForum(forumId) == false)
            {
                return this.Forbid();
            }

            return this.Ok(await this.posts.List(forumId, startAt));
        }

        [HttpGet, Route("api/1.0/forums/posts/popular")]
        [ProducesResponseType(200, Type = typeof(ListResultModel<ForumPostListItemModel>))]
        public async Task<IActionResult> Popular(int startAt, [FromQuery] string sort)
        {
            return this.Ok(await this.views.Popular(startAt, sort));
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        [ProducesResponseType(200, Type = typeof(ForumPostViewModel))]
        public async Task<IActionResult> Get(int forumId, int postId)
        {
            if (await this.forums.CanAccessForum(forumId) == false)
            {
                return this.Forbid();
            }

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
            if (await this.forums.CanAccessForum(forumId) == false)
            {
                return this.Forbid();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            if (await this.forums.GetMemberStatus(forumId) == ForumMemberStatuses.MUTED)
            {
                return this.Forbid();
            }

            return this.Ok(await this.posts.Create(forumId, model));
        }

        [HttpPut, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        public async Task<IActionResult> Update(int forumId, int postId, [FromBody] EditForumPostModel model)
        {
            if (await this.forums.CanAccessForum(forumId) == false)
            {
                return this.Forbid();
            }

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
            if (await this.forums.CanAccessForum(forumId) == false)
            {
                return this.Forbid();
            }

            if (await this.posts.Remove(forumId, postId) == false)
            {
                return this.NotFound();
            }

            return this.Ok();
        }
    }
}
