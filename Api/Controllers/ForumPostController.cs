using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class ForumPostController : Controller
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identity;

        public ForumPostController(
            YARCContext context,
            IMapper mapper,
            IIdentityService identity)
        {
            this.context = context;
            this.mapper = mapper;
            this.identity = identity;
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts")]
        [ProducesResponseType(200, Type = typeof(ForumPostListItemModel[]))]
        public async Task<IActionResult> List(int forumId, int startAt)
        {
            var userId = 0;

            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.identity.GetIdentity().Id;
            }

            var posts = await this.context
                .Posts
                .Where(E => E.ForumId == forumId && E.Id > startAt && E.IsHidden == false)
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
                    Vote = E.Votes.FirstOrDefault(V => V.ById == userId).Vote
                })
                .ToArrayAsync();

            return this.Ok(posts);
        }

        [HttpGet, Route("api/1.0/forums/posts/popular")]
        [ProducesResponseType(200, Type = typeof(ForumPostListItemModel[]))]
        public async Task<IActionResult> Popular(int startAt)
        {
            var userId = 0; 
            
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.identity.GetIdentity().Id;
            }

            var posts = await this.context
                .Posts
                //.Where(E => E.Id > startAt)
                //.OrderBy(E => Guid.NewGuid())
                //.Take(25)
                .Where(E => E.ForumId == 27 && E.IsHidden == false)
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
                    Vote = E.Votes.FirstOrDefault(V => V.ById == userId).Vote
                })
                .Take(25)
                .ToArrayAsync();

            return this.Ok(posts);
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        [ProducesResponseType(200, Type = typeof(ForumPostViewModel))]
        public async Task<IActionResult> Get(int forumId, int postId)
        {
            var userId = 0;

            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.identity.GetIdentity().Id;
            }

            var model = await this.context
                .Posts
                .Where(E => E.Id == postId && E.ForumId == forumId)
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

            if (model == null)
            {
                return this.NotFound();
            }

            return this.Ok(model);
        }

        [HttpPost, Route("api/1.0/forums/{forumId}/posts")]
        [ProducesResponseType(200, Type = typeof(IdModel<int>))]
        public async Task<IActionResult> Create(int forumId, [FromBody] EditForumPostModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = this.mapper.Map<Post>(model);

            entity.ForumId = forumId;
            entity.PostedById = 1;
            entity.CreatedOn = DateTime.UtcNow;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            return this.Ok(new IdModel<int> { Id = entity.Id });
        }

        [HttpPut, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        public async Task<IActionResult> Update(int forumId, int postId, [FromBody] EditForumPostModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = await this.context
                .Posts
                .Where(E => E.ForumId == forumId && E.Id == postId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return NotFound();
            }

            entity.Title = model.Title;
            entity.Text = model.Text;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpDelete, Route("api/1.0/forums/{forumId}/posts/{postId}")]
        public async Task<IActionResult> Remove(int forumId, int postId)
        {
            var entity = await this.context
                .Posts
                .Where(E => E.ForumId == forumId && E.Id == postId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            this.context.Remove(entity);

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
