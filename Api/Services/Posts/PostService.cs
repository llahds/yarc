using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using Api.Services.FullText;
using Api.Services.FullText.Documents;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identity;
        private readonly IFullTextIndex fts;

        public PostService(
            YARCContext context,
            IMapper mapper,
            IIdentityService identity,
            IFullTextIndex fts)
        {
            this.context = context;
            this.mapper = mapper;
            this.identity = identity;
            this.fts = fts;
        }

        public async Task<ListResultModel<ForumPostListItemModel>> List(int forumId, int startAt, string sort)
        {
            var userId = this.identity.GetIdentity()?.Id ?? 0;

            var sortField = "Top DESC";

            if (sort == "top" || string.IsNullOrEmpty(sort))
            {
                sortField = "Top DESC";
                sort = "top";
            }
            else if (sort == "hot")
            {
                sortField = "Hot DESC";
                sort = "hot";
            }
            else if (sort == "new")
            {
                sortField = "New";
                sort = "new";
            }
            else if (sort == "rising")
            {
                sortField = "Rising DESC";
                sort = "rising";
            }

            var pageSize = 25;

            var where = this.context
                .Posts
                .Where(P =>
                    P.ForumId == forumId
                    && P.IsDeleted == false
                    && P.IsHidden == false);

            var total = await where.CountAsync();

            var posts = await where
                .OrderBy(sortField)
                .Skip(startAt)
                .Take(pageSize)
                .Select(E => new ForumPostListItemModel
                {
                    Id = E.Id,
                    CreatedOn = E.CreatedOn,
                    Title = E.Title,
                    Forum = new KeyValueModel
                    {
                        Id = E.ForumId,
                        Name = E.Forum.Name
                    },
                    PostedBy = new PostedByModel
                    {
                        Id = E.PostedById,
                        Name = E.PostedBy.DisplayName ?? "[deleted]",
                        AvatarId = -1
                    },
                    Ups = E.Ups,
                    Downs = E.Downs,
                    Vote = E.Votes.FirstOrDefault(V => V.ById == userId).Vote,
                    CommentCount = E.CommentCount
                })
                .ToArrayAsync();

            return new ListResultModel<ForumPostListItemModel>
            {
                List = posts,
                SortBy = sort,
                Total = total,
                PageSize = pageSize
            };
        }

        public async Task<ForumPostListItemModel> Get(int forumId, int postId)
        {
            var userId = this.identity.GetIdentity()?.Id ?? 0;

            var model = await this.context
                .Posts
                .Where(E =>
                    E.Id == postId
                    && E.ForumId == forumId)
                .Select(E => new ForumPostViewModel
                {
                    Id = E.Id,
                    CreatedOn = E.CreatedOn,
                    Title = E.Title,
                    Text = E.Text,
                    Forum = new KeyValueModel
                    {
                        Id = E.ForumId,
                        Name = E.Forum.Name
                    },
                    PostedBy = new PostedByModel
                    {
                        Id = E.PostedById,
                        Name = E.PostedBy.DisplayName ?? "[deleted]",
                        AvatarId = -1
                    },
                    CanReport = userId > 0
                        && E.ReportedPosts.Any(R => R.ReportedById == userId) == false,
                    Ups = E.Votes.Count(V => V.Vote > 0),
                    Downs = E.Votes.Count(V => V.Vote < 0),
                    Vote = E.Votes.FirstOrDefault(V => V.ById == userId).Vote
                })
                .FirstOrDefaultAsync();

#pragma warning disable CS8603 // Possible null reference return.
            return model;
#pragma warning restore CS8603 // Possible null reference return.
        }

        [Authorize]
        public async Task<IdModel<int>> Create(int forumId, EditForumPostModel model)
        {
            var entity = this.mapper.Map<Post>(model);

            entity.ForumId = forumId;
            entity.PostedById = 1;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            await this.context.AddAsync(new PostVote
            {
                CreatedOn = DateTime.UtcNow,
                ById = this.identity.GetIdentity().Id,
                Post = entity,
                Vote = 1
            });

            await this.context.SaveChangesAsync();

            var forum = await this.context.Forums.FirstOrDefaultAsync(F => F.Id == forumId);

            this.fts.Save(new PostFTS
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                ForumName = forum?.Name,
                ForumId = forum?.Id ?? 0
            });

            return new IdModel<int> { Id = entity.Id };
        }

        public async Task<bool> Update(int forumId, int postId, EditForumPostModel model)
        {
            var entity = await this.context
                .Posts
                .Where(E => E.ForumId == forumId && E.Id == postId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            entity.Title = model.Title;
            entity.Text = model.Text;

            await this.context.SaveChangesAsync();

            var forum = await this.context.Forums.FirstOrDefaultAsync(F => F.Id == forumId);

            this.fts.Save(new PostFTS
            {
                Id = entity.Id,
                Title = entity.Title,
                Text = entity.Text,
                ForumName = forum?.Name,
                ForumId = forum?.Id ?? 0
            });

            return true;
        }

        public async Task<bool> Remove(int forumId, int postId)
        {
            var entity = await this.context
                .Posts
                .Where(E => E.ForumId == forumId && E.Id == postId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;

            await this.context.SaveChangesAsync();

            this.fts.Delete(new PostFTS
            {
                Id = entity.Id,
            });

            return true;
        }

        public async Task VoteUp(int forumId, int postId)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context
                .PostVotes
                .Where(P => P.PostId == postId && P.Post.ForumId == forumId && P.ById == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new PostVote
                {
                    CreatedOn = DateTime.UtcNow,
                    PostId = postId,
                    ById = this.identity.GetIdentity().Id
                };

                await this.context.AddAsync(entity);
            }

            entity.Vote = 1;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.SaveChangesAsync();
        }

        public async Task VoteDown(int forumId, int postId)
        {
            var userId = this.identity.GetIdentity().Id;

            var entity = await this.context
                .PostVotes
                .Where(P => P.PostId == postId && P.Post.ForumId == forumId && P.ById == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new PostVote
                {
                    CreatedOn = DateTime.UtcNow,
                    PostId = postId,
                    ById = this.identity.GetIdentity().Id
                };

                await this.context.AddAsync(entity);
            }

            entity.Vote = -1;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.SaveChangesAsync();
        }
    }
}
