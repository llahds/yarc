using Api.Data;
using Api.Data.Entities;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ForumController : Controller
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;

        public ForumController(
            YARCContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet, Route("api/1.0/forums/{id}/posts/guide-lines")]
        [ProducesResponseType(200, Type = typeof(ForumPostGuideLinesModel))]
        public async Task<IActionResult> GetGuideLines(int id)
        {
            var entity = await this.context
                .Forums
                .Where(E => E.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            var model = this.mapper.Map<ForumPostGuideLinesModel>(entity.PostSettings);

            return this.Ok(model);
        }

        [HttpGet, Route("api/1.0/forums/{id}")]
        [ProducesResponseType(200, Type = typeof(ForumModel))]
        public async Task<IActionResult> Get(int id)
        {
            var model = await this.context
                .Forums
                .Where(E => E.Id == id)
                .Select(F => new ForumModel
                {
                    Name = F.Name,
                    CreatedOn = F.CreatedOn,
                    Description = F.Description, 
                    Slug = F.Slug,
                    Topics = F.Topics.Select(T => new KeyValueModel
                    {
                        Id = T.TopicId,
                        Name = T.Topic.Name
                    })
                    .ToArray()
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        [HttpPost, Route("api/1.0/forums")]
        [ProducesResponseType(200, Type = typeof(IdModel<int>))]
        public async Task<IActionResult> Create([FromBody] EditForumModel model)
        {
            if (string.IsNullOrEmpty(model?.Slug) == false)
            {
                var userNameAlreadyExists = await context
                    .Forums
                    .AnyAsync(U => U.Slug == model.Slug);

                if (userNameAlreadyExists)
                {
                    this.ModelState.AddModelError(nameof(model.Slug), "Slug already exists.");
                }
            }

            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = new Forum();
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Slug = model.Slug;
            entity.PostSettings = new ForumPostSettings();
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            await this.context.AddChildren<ForumTopic, Topic>(
                () => model.Topics.Select(M => M.Id),
                topic => new ForumTopic
                {
                    Topic = topic,
                    Forum = entity
                });

            await this.context.SaveChangesAsync();

            return this.Ok( new IdModel<int> { Id = entity.Id });
        }

        [HttpPut, Route("api/1.0/forums/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditForumModel model)
        {
            if (string.IsNullOrEmpty(model?.Slug) == false)
            {
                var userNameAlreadyExists = await context
                    .Forums
                    .AnyAsync(U => U.Slug == model.Slug && U.Id != id);

                if (userNameAlreadyExists)
                {
                    this.ModelState.AddModelError(nameof(model.Slug), "Slug already exists.");
                }
            }

            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = await this.context
                .Forums
                .Where(E => E.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Slug = model.Slug;

            await this.context.SynchronizeChildren<ForumTopic, Topic>(
                E => E.ForumId == id,
                () => model.Topics.Select(M => M.Id),
                topic => new ForumTopic
                {
                    Forum = entity,
                    Topic = topic
                });

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpDelete, Route("api/1.0/forums/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var entity = await this.context
                .Forums
                .Where(E => E.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            entity.IsDeleted = true;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpGet, Route("api/1.0/forums/topics/suggest")]
        [ProducesResponseType(200, Type = typeof(KeyValueModel[]))]
        public async Task<IActionResult> SuggestTopics(string queryText)
        {
            var model = await this.context
                .Topics
                .Where(E => E.Name.StartsWith(queryText))
                .Select(F => new KeyValueModel
                {
                    Id = F.Id,
                    Name = F.Name
                })
                .OrderBy(B => B.Name)
                .ToArrayAsync();

            return this.Ok(model);
        }
    }
}
