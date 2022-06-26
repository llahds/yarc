using Api.Models;
using Api.Services.FullText;
using Api.Services.FullText.Documents;
using Lucene.Net.Analysis.En;
using Lucene.Net.QueryParsers.Classic;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SearchController : Controller
    {
        private readonly IFullTextIndex fts;

        public SearchController(
            IFullTextIndex fts)
        {
            this.fts = fts;
        }

        [HttpGet, Route("api/1.0/search/overview")]
        [ProducesResponseType(200, Type = typeof(SearchGroupResultModel[]))]
        public async Task<IActionResult> SearchOverview([FromQuery] string query)
        {
            var qp = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "__text", new EnglishAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48));
            var q = qp.Parse(query);

            var resultGroups = new List<SearchGroupResultModel>();

            var forums = this.fts.Search<ForumFTS>(q, null, 0, 10, 1000);

            resultGroups.Add(new SearchGroupResultModel
            {
                Type = "Forums",
                Total = forums.Total,
                Results = forums.Results
            }); ;

            var posts = this.fts.Search<PostFTS>(q, null, 0, 10, 1000);

            resultGroups.Add(new SearchGroupResultModel
            {
                Type = "Posts",
                Total = posts.Total,
                Results = posts.Results
            });

            var comments = this.fts.Search<CommentFTS>(q, null, 0, 10, 1000);

            resultGroups.Add(new SearchGroupResultModel
            {
                Type = "Comments",
                Total = comments.Total,
                Results = comments.Results
            });

            return this.Ok(resultGroups);
        }

        [HttpGet, Route("api/1.0/search/{type}")]
        [ProducesResponseType(200, Type = typeof(SearchGroupResultModel))]
        public async Task<IActionResult> Search(string type, [FromQuery] string query, [FromQuery] int startAt)
        {
            var qp = new QueryParser(Lucene.Net.Util.LuceneVersion.LUCENE_48, "__text", new EnglishAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48));
            var q = qp.Parse(query);

            if (type == "forums")
            {
                var forums = this.fts.Search<ForumFTS>(q, null, startAt, 10, 1000);

                return this.Ok(new SearchGroupResultModel
                {
                    Type = "Forums",
                    Total = forums.Total,
                    Results = forums.Results
                });
            }

            if (type == "posts")
            {
                var posts = this.fts.Search<PostFTS>(q, null, startAt, 10, 1000);

                return this.Ok(new SearchGroupResultModel
                {
                    Type = "Posts",
                    Total = posts.Total,
                    Results = posts.Results
                });
            }

            if (type == "comments")
            {
                var comments = this.fts.Search<CommentFTS>(q, null, startAt, 10, 1000);

                return this.Ok(new SearchGroupResultModel
                {
                    Type = "Comments",
                    Total = comments.Total,
                    Results = comments.Results
                });
            }

            return this.Ok();
        }
    }
}
