using Api.Data.Entities;
using Api.Models;
using Api.Services.Forums;
using Api.Services.Moderation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumService forums;
        private readonly IModerationService moderation;

        public ForumController(
            IForumService forums,
            IModerationService moderation)
        {
            this.forums = forums;
            this.moderation = moderation;
        }

        [HttpGet, Route("api/1.0/forums/{id}/access")]
        [ProducesResponseType(200, Type = typeof(CanAccessForumModel))]
        public async Task<IActionResult> CheckAccess(int id)
        {
            return this.Ok(new CanAccessForumModel
            {
                CanAccessForum = await this.forums.CanAccessForum(id)
            });
        }

        [HttpGet, Route("api/1.0/forums/{id}/posts/guide-lines")]
        [ProducesResponseType(200, Type = typeof(ForumPostGuideLinesModel))]
        public async Task<IActionResult> GetGuideLines(int id)
        {
            if (await this.forums.CanAccessForum(id) == false)
            {
                return this.Forbid();
            }

            var model = await this.forums.GetForumGuidelines(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        [HttpGet, Route("api/1.0/forums/{id}")]
        [ProducesResponseType(200, Type = typeof(ForumModel))]
        public async Task<IActionResult> Get(int id)
        {
            if (await this.forums.CanAccessForum(id) == false)
            {
                return this.Forbid();
            }

            var model = await this.forums.Get(id);

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        [Authorize]
        [HttpPost, Route("api/1.0/forums")]
        [ProducesResponseType(200, Type = typeof(IdModel<int>))]
        public async Task<IActionResult> Create([FromBody] EditForumModel model)
        {
            if (string.IsNullOrEmpty(model?.Slug) == false)
            {
                if (await this.forums.SlugAlreadyTaken(model.Slug, null))
                {
                    this.ModelState.AddModelError(nameof(model.Slug), "Slug already exists.");
                }
            }

            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(await this.forums.Create(model));
        }

        [Authorize]
        [HttpPut, Route("api/1.0/forums/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditForumModel model)
        {
            if (await this.moderation.IsOwner(id) == false)
            {
                return this.Forbid();
            }

            if (string.IsNullOrEmpty(model?.Slug) == false)
            {
                if (await this.forums.SlugAlreadyTaken(model.Slug, id))
                {
                    this.ModelState.AddModelError(nameof(model.Slug), "Slug already exists.");
                }
            }

            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            if (await this.forums.Update(id, model) == false)
            {
                return NotFound();
            }

            return this.Ok();
        }

        [HttpDelete, Route("api/1.0/forums/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (await this.moderation.IsOwner(id) == false)
            {
                return this.Forbid();
            }

            if (await this.forums.Remove(id))
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpGet, Route("api/1.0/forums/topics/suggest")]
        [ProducesResponseType(200, Type = typeof(KeyValueModel[]))]
        public async Task<IActionResult> SuggestTopics(string queryText)
        {
            return this.Ok(await this.forums.SuggestTopics(queryText));
        }

        [HttpGet, Route("api/1.0/forums/users/suggest")]
        [ProducesResponseType(200, Type = typeof(KeyValueModel[]))]
        public async Task<IActionResult> SuggestUsers(string queryText)
        {
            return this.Ok(await this.forums.SuggestUsers(queryText));
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/similar")]
        [ProducesResponseType(200, Type = typeof(SimilarForumModel[]))]
        public async Task<IActionResult> FindSimilarForums(int forumId)
        {
            return this.Ok(await this.forums.FindSimilarForums(forumId));
        }

        [Authorize]
        [HttpPost, Route("api/1.0/forums/{forumId}/join")]
        public async Task<IActionResult> Join(int forumId)
        {
            var statusId = await this.forums.GetMemberStatus(forumId);

            if (statusId == -1 || statusId == ForumMemberStatuses.LEFT)
            {
                await this.forums.Join(forumId);

                return this.Ok();
            }

            return this.NoContent();            
        }

        [Authorize]
        [HttpPost, Route("api/1.0/forums/{forumId}/leave")]
        public async Task<IActionResult> Leave(int forumId)
        {
            var statusId = await this.forums.GetMemberStatus(forumId);

            if (statusId == -1 || statusId == ForumMemberStatuses.JOINED || statusId == ForumMemberStatuses.APPROVED)
            {
                await this.forums.Leave(forumId);

                return this.Ok();
            }

            return this.NoContent();
        }

        [HttpGet, Route("api/1.0/forums/slug/{slug}")]
        [ProducesResponseType(200, Type = typeof(IdModel<int>))]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var r = await this.forums.GetIdBySlug(slug);

            if (r == null)
            {
                return this.NotFound();
            }

            return this.Ok(new IdModel<int> { Id = r.Value });
        }

        [HttpGet, Route("api/1.0/forums/popular")]
        [ProducesResponseType(200, Type = typeof(SimilarForumModel[]))]
        public async Task<IActionResult> GetPopularForums()
        {
            return this.Ok(await this.forums.GetPopularForums());
        }

    }
}
