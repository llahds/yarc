using Api.Models;
using Api.Services.Forums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForumService forums;

        public ForumController(
            IForumService forums)
        {
            this.forums = forums;
        }

        [HttpGet, Route("api/1.0/forums/{id}/posts/guide-lines")]
        [ProducesResponseType(200, Type = typeof(ForumPostGuideLinesModel))]
        public async Task<IActionResult> GetGuideLines(int id)
        {
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
            if (await this.forums.VerifyCredentials(id) == false)
            {
                return this.Unauthorized();
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
            if (await this.forums.VerifyCredentials(id) == false)
            {
                return this.Unauthorized();
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
        public async Task<IActionResult> FindSimilarTopics(int forumId)
        {
            return this.Ok(await this.forums.FindSimilarForums(forumId));
        }
    }
}
