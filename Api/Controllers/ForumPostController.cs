using Api.Data.Entities;
using Api.Models;
using Api.Services.BackgroundJobs;
using Api.Services.Forums;
using Api.Services.Posts;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly IPostService posts;
        private readonly IPostViewService views;
        private readonly IForumService forums;
        private readonly IPostValidationService validator;
        private readonly IBackgroundJobClient backgroundJob;

        public ForumPostController(
            IPostService posts,
            IPostViewService views,
            IForumService forums,
            IPostValidationService validator,
            IBackgroundJobClient backgroundJob)
        {
            this.posts = posts;
            this.views = views;
            this.forums = forums;
            this.validator = validator;
            this.backgroundJob = backgroundJob;
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts")]
        [ProducesResponseType(200, Type = typeof(ListResultModel<ForumPostListItemModel>))]
        public async Task<IActionResult> List(int forumId, int startAt, string sort)
        {
            if (await this.forums.CanAccessForum(forumId) == false)
            {
                return this.Forbid();
            }

            return this.Ok(await this.posts.List(forumId, startAt, sort));
        }

        [HttpGet, Route("api/1.0/forums/posts/popular")]
        [ProducesResponseType(200, Type = typeof(ListResultModel<ForumPostListItemModel>))]
        public async Task<IActionResult> Popular(int startAt, string sort)
        {
            return this.Ok(await this.views.Popular(startAt, sort));
        }

        [Authorize]
        [HttpGet, Route("api/1.0/forums/posts/recommended")]
        [ProducesResponseType(200, Type = typeof(ListResultModel<ForumPostListItemModel>))]
        public async Task<IActionResult> Recommended(int startAt)
        {
            return this.Ok(await this.views.Recommended(startAt));
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

        [Authorize]
        [HttpPost, Route("api/1.0/forums/{forumId}/posts")]
        [ProducesResponseType(200, Type = typeof(IdModel<int>))]
        public async Task<IActionResult> Create(int forumId, [FromBody] EditForumPostModel model)
        {
            if (await this.forums.CanAccessForum(forumId) == false)
            {
                return this.Forbid();
            }

            if (await this.forums.GetMemberStatus(forumId) == ForumMemberStatuses.MUTED)
            {
                return this.Forbid();
            }

            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var result = await this.validator.ValidatePost(forumId, model);

            if (result != null)
            {
                this.ModelState.AddModelError(result.Field, result.Message);
                return this.BadRequest(this.ModelState);
            }

            var id = await this.posts.Create(forumId, model);

            this.backgroundJob.Enqueue<CheckPostToxicity>(item => item.Check(id.Id));

            this.backgroundJob.Enqueue<CheckPostSpam>(item => item.Check(id.Id));

            return this.Ok(id);
        }

        [Authorize]
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

            var result = await this.validator.ValidatePost(forumId, model);

            if (result != null)
            {
                this.ModelState.AddModelError(result.Field, result.Message);
                return this.BadRequest(this.ModelState);
            }

            if (await this.posts.Update(forumId, postId, model) == false)
            {
                return this.NotFound();
            }

            this.backgroundJob.Enqueue<CheckPostToxicity>(item => item.Check(postId));

            this.backgroundJob.Enqueue<CheckPostSpam>(item => item.Check(postId));

            return this.Ok();
        }

        [Authorize]
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
