using Api.Data;
using Api.Data.Entities;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;

        public ForumPostController(
            YARCContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        [ProducesResponseType(200, Type = typeof(ForumPostModel))]
        public async Task<IActionResult> Get(int forumId, int postId)
        {
            var entity = await this.context
                .Posts
                .Where(E => E.Id == postId && E.ForumId == forumId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            var model = this.mapper.Map<ForumPostModel>(entity);

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

            var entity = this.mapper.Map<Post>(model);

            entity.ForumId = forumId;
            entity.PostedById = 1;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            return this.Ok(new IdModel<int> { Id = entity.Id });
        }

        [HttpPut, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        public async Task<IActionResult> Update(int forumId, int postId, [FromBody] EditForumPostModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = await this.context
                .Posts
                .Where(E => E.ForumId == forumId && E.Id == postId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            entity.Title = model.Title;
            entity.Text = model.Text;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpDelete, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        public async Task<IActionResult> Remove(int forumId, int postId)
        {
            var entity = await this.context
                .Posts
                .Where(E => E.ForumId == forumId && E.Id == postId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            this.context.Remove(entity);

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
