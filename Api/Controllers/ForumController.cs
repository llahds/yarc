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

        [HttpGet, Route("api/1.0/forums/{id}")]
        [ProducesResponseType(200, Type = typeof(ForumModel))]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await this.context
                .Forums
                .Where(E => E.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            var model = this.mapper.Map<ForumModel>(entity);

            return this.Ok(model);
        }

        [HttpPost, Route("api/1.0/forums")]
        [ProducesResponseType(200, Type = typeof(IdModel<int>))]

        public async Task<IActionResult> Create([FromBody] EditForumModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = this.mapper.Map<Forum>(model);

            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            return this.Ok( new IdModel<int> { Id = entity.Id });
        }

        [HttpPut, Route("api/1.0/forums/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EditForumModel model)
        {
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

            this.context.Remove(entity);

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
