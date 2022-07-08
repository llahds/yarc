using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Reporting
{
    public class ReportingService : IReportingService
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identity;

        public ReportingService(
            YARCContext context,
            IMapper mapper,
            IIdentityService identity)
        {
            this.context = context;
            this.mapper = mapper;
            this.identity = identity;
        }

        public async Task<ReportReasonModel[]> GetReportReasons()
        {
            var entities = await this.context
                .ReportReasons
                .ToArrayAsync();

            return entities.Select(E => this.mapper.Map<ReportReasonModel>(E)).ToArray();
        }

        public async Task CreateFromPost(ReportedPostModel model)
        {
            var entity = new ReportedPost();

            entity.PostId = model.PostId;
            entity.ReasonId = model.ReasonId;
            // if authenticated identity can't be found then default to service accout (yarcbot)
            entity.ReportedById = this.identity.GetIdentity()?.Id ?? -1;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            var post = await this.context.Posts.FirstOrDefaultAsync(P => P.Id == model.PostId);

            post.IsHidden = true;

            await this.context.SaveChangesAsync();
        }

        public async Task CreateFromComment(ReportedCommentModel model)
        {
            var entity = new ReportedComment();

            entity.CommentId = model.CommentId;
            entity.ReasonId = model.ReasonId;
            entity.ReportedById = this.identity.GetIdentity().Id;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            var comment = await this.context
                .Comments
                .FirstOrDefaultAsync(P => P.Id == model.CommentId);

            comment.IsHidden = true;

            await this.context.SaveChangesAsync();
        }
    }
}
