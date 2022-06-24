﻿using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identity;

        public PostService(
            YARCContext context,
            IMapper mapper,
            IIdentityService identity)
        {
            this.context = context;
            this.mapper = mapper;
            this.identity = identity;
        }

        public async Task<ForumPostListItemModel[]> List(int forumId, int startAt)
        {
            var userId = this.identity.GetIdentity()?.Id ?? 0;

            var posts = await this.context
                .Posts
                .Where(E =>
                    E.ForumId == forumId
                    && E.Id > startAt
                    && E.IsHidden == false
                    && E.IsDeleted == false)
                .OrderBy(E => E.CreatedOn)
                .Take(25)
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
                    Ups = E.Votes.Count(V => V.Vote > 0),
                    Downs = E.Votes.Count(V => V.Vote < 0),
                    Vote = E.Votes.FirstOrDefault(V => V.ById == userId).Vote,
                    CommentCount = E.Comments.Count(C => !C.IsDeleted || !C.IsHidden)
                })
                .ToArrayAsync();

            return posts;
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

        public async Task<IdModel<int>> Create(int forumId, EditForumPostModel model)
        {
            var entity = this.mapper.Map<Post>(model);

            entity.ForumId = forumId;
            entity.PostedById = 1;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

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

            await this.context.SaveChangesAsync();
        }
    }
}