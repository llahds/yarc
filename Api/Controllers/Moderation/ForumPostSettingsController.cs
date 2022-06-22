using Api.Data;
using Api.Data.Entities;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers.Moderation
{
    [Authorize]
    public class ForumPostSettingsController : Controller
    {
        private readonly IMapper mapper;
        private readonly YARCContext context;

        public ForumPostSettingsController(
            IMapper mapper,
            YARCContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet, Route("api/1.0/moderation/forums/{forumId}/posts/settings")]
        [ProducesResponseType(200, Type = typeof(ForumPostSettingsModel))]
        public async Task<IActionResult> Get(int forumId)
        {
            var entity = await this.context
                .Forums
                .FirstOrDefaultAsync(F => F.Id == forumId);

            var model = this.mapper.Map<ForumPostSettingsModel>(entity.PostSettings);

            return this.Ok(model);
        }

        [HttpPut, Route("api/1.0/moderation/forums/{forumId}/posts/settings")]
        public async Task<IActionResult> Update(int forumId, [FromBody] ForumPostSettingsModel model)
        {
            var entity = await this.context
                .Forums
                .FirstOrDefaultAsync(F => F.Id == forumId);

            var settings = this.mapper.Map<ForumPostSettings>(model);

            entity.PostSettings = settings;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
