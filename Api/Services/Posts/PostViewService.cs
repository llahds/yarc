using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using Api.Services.FullText;
using Api.Services.FullText.Documents;
using Api.Services.Text;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.Services.Posts
{
    public class PostViewService : IPostViewService
    {
        private readonly IIdentityService identity;
        private readonly YARCContext context;
        private readonly IFullTextIndex index;

        public PostViewService(
            IIdentityService identity,
            YARCContext context,
            IFullTextIndex index)
        {
            this.identity = identity;
            this.context = context;
            this.index = index;
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

        public async Task<ListResultModel<ForumPostListItemModel>> Recommended(int startAt)
        {
            var userId = this.identity.GetIdentity()?.Id ?? 0;

            var setting = await this.context.UserSettings
                .Where(U => U.UserId == userId && U.Key == "RECOMMEND_POST_QUERY")
                .FirstOrDefaultAsync();

            if (setting == null)
            {
                return new ListResultModel<ForumPostListItemModel>();
            }

            var query = JsonConvert.DeserializeObject<KeywordScorer.Topic[]>(setting.Value);

            var bq = new BooleanQuery();

            foreach (var up in query[0].Keywords.Take(25))
            {
                var tq = new TermQuery(new Term("__text", up.Token));
                tq.Boost = (float)up.Score;
                bq.Add(tq, Occur.SHOULD);
            }

            foreach (var down in query[1].Keywords.Take(25))
            {
                var tq = new TermQuery(new Term("__text", down.Token));
                tq.Boost = (float)down.Score;
                bq.Add(tq, Occur.SHOULD);
            }

            var hits = this.index.Search<PostFTS>(bq, null, startAt, 25, 1000);

            var searchScores = hits
                .Results
                .Select((R, I) => new { Id = R.Id, Score = hits.Scores[I] })
                .ToDictionary(R => R.Id, V => V.Score);

            var ids = hits.Results.Select(R => R.Id).ToArray();

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
                    && ids.Contains(P.Id)
                 );

            var posts = await this.context
                .Posts
                .Where(P => P.IsDeleted == false && P.IsHidden == false && P.IsSpam == false && ids.Contains(P.Id))
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

            var sorted = posts
                .OrderByDescending(R =>
                {
                    if (searchScores.ContainsKey(R.Id))
                    {
                        return searchScores[R.Id];
                    }

                    return 0.0;
                })
                .ToArray();

            return new ListResultModel<ForumPostListItemModel>
            {
                List = sorted,
                SortBy = "",
                PageSize = 25,
                Total = hits.Total
            };
        }
    }
}
