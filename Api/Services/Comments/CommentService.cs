using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using Api.Services.FullText;
using Api.Services.FullText.Documents;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly IIdentityService identity;
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IFullTextIndex fts;

        public CommentService(
            IIdentityService identity,
            YARCContext context,
            IMapper mapper,
            IFullTextIndex fts)
        {
            this.identity = identity;
            this.context = context;
            this.mapper = mapper;
            this.fts = fts;
        }

        public async Task<CommentModel[]> GetComments(int forumId, int postId, int? parentId)
        {
            var userId = this.identity.GetIdentity()?.Id ?? 0;

            var comments = await this.context
                .Comments
                .Where(E =>
                    E.Post.ForumId == forumId
                    && E.PostId == postId
                    && E.ParentId == parentId
                    && E.IsDeleted == false
                    && E.IsHidden == false)
                .OrderBy(E => E.CreatedOn)
                .Select(E => new CommentModel
                {
                    Id = E.Id,
                    CreatedOn = E.CreatedOn,
                    Text = E.Text,
                    PostedBy = new PostedByModel
                    {
                        Id = E.PostedById,
                        Name = E.PostedBy.DisplayName ?? "[deleted]",
                        AvatarId = -1
                    },
                    ReplyCount = E.Children.Count(),
                    Ups = E.Votes.Count(V => V.Vote > 0),
                    Downs = E.Votes.Count(V => V.Vote < 0),
                    Vote = E.Votes.FirstOrDefault(V => V.ById == userId).Vote,
                    CanReport = userId > 0
                        && E.ReportedComments.Any(R => R.ReportedById == userId) == false,
                })
                .ToArrayAsync();

            return comments;
        }

        public async Task<CommentModel> CreateAtRoot(int forumId, int postId, CommentEditModel model)
        {
            var entity = this.mapper.Map<Comment>(model);
            entity.PostId = postId;
            entity.ParentId = null;
            entity.CreatedOn = DateTime.UtcNow;
            entity.PostedById = this.identity.GetIdentity().Id;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            this.fts.Save(new CommentFTS
            {
                Id = entity.Id,
                Text = entity.Text,
                ForumId = forumId,
                PostId = postId
            });

            return await this.context
                .Comments
                .Where(E => E.Id == entity.Id)
                .Select(E => new CommentModel
                {
                    Id = E.Id,
                    CreatedOn = E.CreatedOn,
                    Text = E.Text,
                    PostedBy = new PostedByModel
                    {
                        Id = E.PostedById,
                        Name = E.PostedBy.DisplayName ?? "[deleted]",
                        AvatarId = -1
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CommentModel> CreateAtParent(int forumId, int postId, int parentId, CommentEditModel model)
        {
            var entity = this.mapper.Map<Comment>(model);
            entity.PostId = postId;
            entity.ParentId = null;
            entity.CreatedOn = DateTime.UtcNow;
            entity.PostedById = this.identity.GetIdentity().Id;
            entity.ParentId = parentId;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            this.fts.Save(new CommentFTS
            {
                Id = entity.Id,
                Text = entity.Text,
                ForumId = forumId,
                PostId = postId
            });

            return await this.context
                .Comments
                .Where(E => E.Id == entity.Id)
                .Select(E => new CommentModel
                {
                    Id = E.Id,
                    CreatedOn = E.CreatedOn,
                    Text = E.Text,
                    PostedBy = new PostedByModel
                    {
                        Id = E.PostedById,
                        Name = E.PostedBy.DisplayName ?? "[deleted]",
                        AvatarId = -1
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Update(int forumId, int postId, int commentId, CommentEditModel model)
        {
            var entity = await this.context
                .Comments
                .Where(C => C.PostId == postId && C.Post.ForumId == forumId && C.Id == commentId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            entity.Text = model.Text;

            await this.context.SaveChangesAsync();

            this.fts.Save(new CommentFTS
            {
                Id = entity.Id,
                Text = entity.Text,
                ForumId = forumId,
                PostId = postId
            });

            return true;
        }

        public async Task<bool> Remove(int forumId, int postId, int commentId)
        {
            var entity = await this.context
                .Comments
                .Where(C => C.PostId == postId && C.Post.ForumId == forumId && C.Id == commentId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;

            await this.context.SaveChangesAsync();

            this.fts.Delete(new CommentFTS
            {
                Id = entity.Id,
            });

            return true;
        }

        public async Task VoteUp(int forumId, int postId, int commentId)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context
                .CommentVotes
                .Where(P => P.CommentId == commentId && P.Comment.PostId == postId && P.Comment.Post.ForumId == forumId && P.ById == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new CommentVote
                {
                    CreatedOn = DateTime.UtcNow,
                    CommentId = commentId,
                    ById = this.identity.GetIdentity().Id
                };

                await this.context.AddAsync(entity);
            }

            entity.Vote = 1;

            await this.context.SaveChangesAsync();
        }

        public async Task VoteDown(int forumId, int postId, int commentId)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context
                .CommentVotes
                .Where(P => P.CommentId == commentId && P.Comment.PostId == postId && P.Comment.Post.ForumId == forumId && P.ById == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new CommentVote
                {
                    CreatedOn = DateTime.UtcNow,
                    CommentId = commentId,
                    ById = this.identity.GetIdentity().Id
                };

                await this.context.AddAsync(entity);
            }

            entity.Vote = -1;

            await this.context.SaveChangesAsync();
        }
    }
}
