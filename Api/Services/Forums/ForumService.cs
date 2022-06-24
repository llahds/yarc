using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Forums
{
    public class ForumService : IForumService
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identity;

        public ForumService(
            YARCContext context,
            IMapper mapper,
            IIdentityService identity)
        {
            this.context = context;
            this.mapper = mapper;
            this.identity = identity;
        }

        public async Task<ForumPostGuideLinesModel> GetForumGuidelines(int forumId)
        {
            var entity = await this.context
                .Forums
                .Where(E => E.Id == forumId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }

            return this.mapper.Map<ForumPostGuideLinesModel>(entity.PostSettings);
        }

        public async Task<ForumModel> Get(int forumId)
        {
            var userId = this.identity.GetIdentity()?.Id ?? 0;

#pragma warning disable CS8603 // Possible null reference return.
            return await this.context
                .Forums
                .Where(E => E.Id == forumId)
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
                    .ToArray(),
                    Moderators = F.ForumModerators.Select(T => new KeyValueModel
                    {
                        Id = T.ModeratorId,
                        Name = T.Moderator.UserName
                    })
                    .ToArray(),
                    MemberCount = F.Members.Count(),
                    IsOwner = (userId > 0 && F.ForumOwners.Any(U => U.OwnerId == userId)),
                    IsModerator = (userId > 0 && F.ForumModerators.Any(U => U.ModeratorId == userId))
                })
                .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<bool> SlugAlreadyTaken(string slug, int? forumId)
        {
            return await this.context
                .Forums
                .Where(F => F.Slug == slug && (forumId == null || F.Id != forumId.Value))
                .AnyAsync();
        }

        public async Task<IdModel<int>> Create(EditForumModel model)
        {
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

            await this.context.AddChildren<ForumModerator, User>(
                () => model.Moderators.Select(M => M.Id),
                moderator => new ForumModerator
                {
                    Moderator = moderator,
                    Forum = entity
                });

            await this.context.AddAsync(new ForumOwner
            {
                Forum = entity,
                OwnerId = this.identity.GetIdentity().Id
            });

            await this.context.SaveChangesAsync();

            return new IdModel<int> { Id = entity.Id };
        }

        public async Task<bool> Update(int forumId, EditForumModel model)
        {
            var entity = await this.context
                .Forums
                .Where(E => E.Id == forumId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Slug = model.Slug;

            await this.context.SynchronizeChildren<ForumTopic, Topic>(
                E => E.ForumId == forumId,
                () => model.Topics.Select(M => M.Id),
                topic => new ForumTopic
                {
                    Forum = entity,
                    Topic = topic
                });

            await this.context.SynchronizeChildren<ForumModerator, User>(
                E => E.ForumId == forumId,
                () => model.Moderators.Select(M => M.Id),
                moderator => new ForumModerator
                {
                    Forum = entity,
                    Moderator = moderator
                });

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Remove(int forumId)
        {
            var entity = await this.context
                .Forums
                .Where(E => E.Id == forumId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> VerifyCredentials(int forumId)
        {
            var userId = this.identity.GetIdentity().Id;

            return await this.context
                .ForumOwners
                .AnyAsync(M => M.OwnerId == userId && M.ForumId == forumId);
        }

        public async Task<KeyValueModel[]> SuggestTopics(string queryText)
        {
            return await this.context
                .Topics
                .Where(E => E.Name.StartsWith(queryText))
                .Select(F => new KeyValueModel
                {
                    Id = F.Id,
                    Name = F.Name
                })
                .OrderBy(B => B.Name)
                .ToArrayAsync();
        }

        public async Task<KeyValueModel[]> SuggestUsers(string queryText)
        {
            return await this.context
                .Users
                .Where(E => E.UserName.StartsWith(queryText))
                .Select(F => new KeyValueModel
                {
                    Id = F.Id,
                    Name = F.UserName
                })
                .OrderBy(B => B.Name)
                .ToArrayAsync();
        }

        public async Task<SimilarForumModel[]> FindSimilarForums(int forumId)
        {
            var forumTopicIds = this.context
                .ForumTopics
                .Where(F => F.ForumId == forumId)
                .Select(F => F.TopicId);

            var similar = await this.context
                .Forums
                .Select(F => new
                {
                    Forum = F,
                    TopicCount = F.Topics.Count(T => forumTopicIds.Contains(T.TopicId))
                })
                .Where(T => T.TopicCount > 0 && T.Forum.Id != forumId)
                .OrderByDescending(F => F.TopicCount)
                .Select(T => new SimilarForumModel
                {
                    Id = T.Forum.Id,
                    Name = T.Forum.Name,
                    MemberCount = T.Forum.Members.Count()
                })
                .Take(10)
                .ToArrayAsync();

            return similar;
        }
    }
}
