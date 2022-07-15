using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Posts
{
    public class PostViewService : IPostViewService
    {
        private readonly IIdentityService identity;
        private readonly YARCContext context;

        public PostViewService(
            IIdentityService identity,
            YARCContext context)
        {
            this.identity = identity;
            this.context = context;
        }

        public async Task<ListResultModel<ForumPostListItemModel>> Popular(int startAt, string sort)
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
                    P.IsDeleted == false
                    && P.IsHidden == false
                    && (
                        P.Forum.IsPrivate == false
                        || (P.Forum.IsPrivate && P.Forum.Members.Any(M => M.MemberId == userId && (M.Status == ForumMemberStatuses.JOINED || M.Status == ForumMemberStatuses.APPROVED))
                       )
                    )
                 );

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
    }
}
