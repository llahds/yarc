using Api.Data;
using Api.Data.Entities;
using Api.Models;
using Api.Services.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class CommentController : Controller
    {
        private readonly YARCContext context;
        private readonly IMapper mapper;
        private readonly IIdentityService identity;

        public CommentController(
            YARCContext context,
            IMapper mapper,
            IIdentityService identity)
        {
            this.context = context;
            this.mapper = mapper;
            this.identity = identity;
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts/{postId}/comments")]
        [ProducesResponseType(200, Type = typeof(CommentModel[]))]
        public async Task<IActionResult> List(int forumId, int postId, int? parentId = null)
        {
            var userId = 0;

            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.identity.GetIdentity().Id;
            }

            var comments = await this.context
                .Comments
                .Where(E => 
                    E.Post.ForumId == forumId 
                    && E.PostId == postId 
                    && E.ParentId == parentId
                    && E.IsDeleted == false)
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
                    Vote = E.Votes.FirstOrDefault(V => V.ById == userId).Vote
                })
                .ToArrayAsync();

            return this.Ok(comments);
        }

        [Authorize]
        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/comments")]
        [ProducesResponseType(200, Type = typeof(CommentModel))]
        public async Task<IActionResult> CreateAtRoot(int forumId, int postId, [FromBody] CommentEditModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = this.mapper.Map<Comment>(model);
            entity.PostId = postId;
            entity.ParentId = null;
            entity.CreatedOn = DateTime.UtcNow;
            entity.PostedById = this.identity.GetIdentity().Id;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            var ret = await this.context
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

            return this.Ok(ret);
        }

        [Authorize]
        [HttpPost, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{parentId}")]
        [ProducesResponseType(200, Type = typeof(CommentModel))]
        public async Task<IActionResult> CreateAtParent(int forumId, int postId, int parentId, [FromBody] CommentEditModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = this.mapper.Map<Comment>(model);
            entity.PostId = postId;
            entity.ParentId = null;
            entity.CreatedOn = DateTime.UtcNow;
            entity.PostedById = this.identity.GetIdentity().Id;
            entity.ParentId = parentId;

            await this.context.AddAsync(entity);

            await this.context.SaveChangesAsync();

            var ret = await this.context
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

            return this.Ok(ret);
        }

        [Authorize]
        [HttpPut, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}")]
        public async Task<IActionResult> Update(int forumId, int postId, int commentId, [FromBody] CommentEditModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            var entity = await this.context
                .Comments
                .Where(C => C.PostId == postId && C.Post.ForumId == forumId && C.Id == commentId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            entity.Text = model.Text;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }

        [HttpDelete, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}")]
        public async Task<IActionResult> Remove(int forumId, int postId, int commentId)
        {
            var entity = await this.context
                .Comments
                .Where(C => C.PostId == postId && C.Post.ForumId == forumId && C.Id == commentId)
                .FirstOrDefaultAsync();

            if (entity == null)
            {
                return this.NotFound();
            }

            entity.IsDeleted = true;

            await this.context.SaveChangesAsync();

            return this.Ok();
        }
    }
}
