using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ReportingController : Controller
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identity;

        public ReportingController(
            YARCContext context,
            IMapper mapper,
            IIdentityService identity)
        {
            this.context = context;
            this.mapper = mapper;
            this.identity = identity;
        }

        [HttpGet, Route("api/1.0/reporting/reasons")]
        [ProducesResponseType(200, Type = typeof(ReportReasonModel[]))]
        public async Task<IActionResult> Get()
        {
            var entities = await this.context
                .ReportReasons
                .ToArrayAsync();

            var model = entities.Select(E => this.mapper.Map<ReportReasonModel>(E)).ToArray();

            return this.Ok(model);
        }

        [Authorize]
        [HttpPost, Route("api/1.0/reporting/posts")]
        public async Task<IActionResult> Create([FromBody] ReportedPostModel model)
        {
            var entity = new ReportedPost();

            entity.PostId = model.PostId;
            entity.ReasonId = model.ReasonId;
            entity.ReportedById = this.identity.GetIdentity().Id;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            var post = await this.context.Posts.FirstOrDefaultAsync(P => P.Id == model.PostId);

            post.IsHidden = true;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
