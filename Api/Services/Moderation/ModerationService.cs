using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Moderation
{
    public class ModerationService : IModerationService
    {
        private readonly IMapper mapper;
        private readonly YARCContext context;
        private readonly IIdentityService identity;

        public ModerationService(
            IMapper mapper,
            YARCContext context,
            IIdentityService identity)
        {
            this.mapper = mapper;
            this.context = context;
            this.identity = identity;
        }

        public async Task<bool> IsOwner(int forumId)
        {
            var userId = this.identity.GetIdentity().Id;

            var isOwner = await this.context
                .ForumOwners
                .AnyAsync(M => M.OwnerId == userId && M.ForumId == forumId);

            return isOwner;
        }

        public async Task<bool> IsModerator(int forumId)
        {
            var userId = this.identity.GetIdentity().Id;

            var isModerator = await this.context
                .ForumModerator
                .AnyAsync(M => M.ModeratorId == userId && M.ForumId == forumId);

            return isModerator;
        }

        public async Task<ForumPostSettingsModel> GetForumPostSettings(int forumId)
        {
            var entity = await this.context
                .Forums
                .FirstOrDefaultAsync(F => F.Id == forumId);

            var model = this.mapper.Map<ForumPostSettingsModel>(entity.PostSettings);

            return model;
        }

        public async Task UpdateForumPostSettings(int forumId, ForumPostSettingsModel model)
        {
            var entity = await this.context
                .Forums
                .FirstOrDefaultAsync(F => F.Id == forumId);

            var settings = this.mapper.Map<ForumPostSettings>(model);

            entity.PostSettings = settings;

            await this.context.SaveChangesAsync();
        }

        public async Task<ReportQueueItemModel[]> GetModerationQueue(int forumId, int startAt, int[] reasonIds)
        {
            var items = await this.context
                .Posts
                .Where(P =>
                    P.ForumId == forumId
                    && P.ReportedPosts.Any(R => reasonIds.Length == 0 || (reasonIds.Length > 0 && reasonIds.Contains(R.ReasonId)))
                    && P.IsDeleted == false)
                .Select(P => new ReportQueueItemModel
                {
                    Post = new ForumPostListItemModel
                    {
                        Id = P.Id,
                        CreatedOn = P.CreatedOn,
                        Title = P.Title,
                        Forum = new KeyValueModel
                        {
                            Id = P.ForumId,
                            Name = P.Forum.Name
                        },
                        PostedBy = new PostedByModel
                        {
                            Id = P.PostedById,
                            Name = P.PostedBy.DisplayName ?? "[deleted]",
                            AvatarId = -1
                        }
                    },
                    Reasons = P
                        .ReportedPosts
                        .GroupBy(R => R.Reason.Label)
                        .Select(R => R.Key)
                        .ToArray()
                })
                .Take(25)
                .ToListAsync();

            var comments = await this.context
                .Comments
                .Where(C =>
                    C.Post.ForumId == forumId
                    && C.ReportedComments.Any(R => reasonIds.Length == 0 || (reasonIds.Length > 0 && reasonIds.Contains(R.ReasonId)))
                    && C.IsDeleted == false
                )
                .Select(C => new ReportQueueItemModel
                {
                    Comment = new CommentInfoModel
                    {
                        Id = C.Id,
                        CreatedOn = C.CreatedOn,
                        Text = C.Text,
                        PostedBy = new PostedByModel
                        {
                            Id = C.PostedById,
                            Name = C.PostedBy.UserName
                        }
                    },
                    Reasons = C
                        .ReportedComments
                        .GroupBy(R => R.Reason.Label)
                        .Select(R => R.Key)
                        .ToArray()
                })
                .Take(25)
                .ToListAsync();

            items.AddRange(comments);

            return items.ToArray();
        }

        public async Task ApprovePost(int forumId, int postId)
        {
            var post = await this.context
                .Posts
                .Include(P => P.ReportedPosts)
                .Where(U =>
                    U.ForumId == forumId
                    && U.Id == postId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (post != null)
            {
                post.IsHidden = false;

                context.RemoveRange(post.ReportedPosts);

                await this.context.SaveChangesAsync();
            }
        }

        public async Task RejectPost(int forumId, int postId)
        {
            var post = await this.context
                .Posts
                .Include(P => P.ReportedPosts)
                .Where(U =>
                    U.ForumId == forumId
                    && U.Id == postId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (post != null)
            {
                post.IsDeleted = true;

                context.RemoveRange(post.ReportedPosts);

                await this.context.SaveChangesAsync();
            }
        }

        public async Task ApproveComment(int forumId, int commentId)
        {
            var comment = await this.context
                .Comments
                .Include(P => P.ReportedComments)
                .Where(U =>
                    U.Post.ForumId == forumId
                    && U.Id == commentId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (comment != null)
            {
                comment.IsHidden = false;

                context.RemoveRange(comment.ReportedComments);

                await this.context.SaveChangesAsync();
            }
        }

        public async Task RejectComment(int forumId, int commentId)
        {
            var comment = await this.context
                .Comments
                .Include(P => P.ReportedComments)
                .Where(U =>
                    U.Post.ForumId == forumId
                    && U.Id == commentId
                    && U.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (comment != null)
            {
                comment.IsDeleted = true;

                context.RemoveRange(comment.ReportedComments);

                await this.context.SaveChangesAsync();
            }
        }

        public async Task<UserInfoModel[]> SearchForumMembers(int forumId, int startAt, string query, int status)
        {
            query = query ?? "";

            var model = await this.context
                .Users
                .Where(U =>
                    U.Forums.Any(F => F.ForumId == forumId && F.Status == status)
                    && (query.Length == 0 || query.Length > 0 && U.UserName.StartsWith(query))
                    && U.IsDeleted == false)
                .Select(U => new UserInfoModel
                {
                    Id = U.Id,
                    UserName = U.UserName
                })
                .Take(25)
                .Skip(startAt)
                .ToArrayAsync();

            return model;
        }

        public async Task<UserInfoModel[]> SearchUsers(string query)
        {
            query = query ?? "";

            var model = await this.context
                .Users
                .Where(U =>
                    (query.Length == 0 || query.Length > 0 && U.UserName.StartsWith(query))
                    && U.IsDeleted == false
                )
                .Select(U => new UserInfoModel
                {
                    Id = U.Id,
                    UserName = U.UserName
                })
                .Take(25)
                .OrderBy(U => U.UserName)
                .ToArrayAsync();

            return model;
        }

        public async Task RemoveForumMember(int forumId, int userId)
        {
            var entity = await this.context
                .ForumMembers
                .Where(U => U.ForumId == forumId && U.MemberId == userId)
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                this.context.Remove(entity);

                await this.context.SaveChangesAsync();
            }
        }

        public async Task ChangeForumMemberStatus(int forumId, int userId, ForumMemberStatusModel model)
        {
            var entity = await this.context
                .ForumMembers
                .Where(U => U.ForumId == forumId && U.Id == userId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new Data.Entities.ForumMember
                {
                    ForumId = forumId,
                    MemberId = userId
                };

                await this.context.AddAsync(entity);
            }

            entity.CreatedOn = DateTime.UtcNow;
            entity.Status = model.Status;

            await this.context.SaveChangesAsync();
        }
    }
}
