using Api.Data;
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

        public async Task<ForumPostListItemModel[]> Popular(int startAt)
        {
            var userId = this.identity.GetIdentity()?.Id ?? 0;

            var posts = await this.context
                .PostScores
                .Include(P => P.Post)
                .OrderByDescending(S => S.Top)
                .Take(25)
                .Select(E => E.Post)
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
    }
}
