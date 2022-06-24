using Api.Models;
using Api.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService comments;

        public CommentController(
            ICommentService comments)
        {
            this.comments = comments;
        }

        [HttpGet, Route("api/1.0/forums/{forumId}/posts/{postId}/comments")]
        [ProducesResponseType(200, Type = typeof(CommentModel[]))]
        public async Task<IActionResult> List(int forumId, int postId, int? parentId = null)
        {
            return this.Ok(await this.comments.GetComments(forumId, postId, parentId));
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

            return this.Ok(await this.comments.CreateAtRoot(forumId, postId, model));
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

            return this.Ok(await this.comments.CreateAtParent(forumId, postId, parentId, model));
        }

        [Authorize]
        [HttpPut, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}")]
        public async Task<IActionResult> Update(int forumId, int postId, int commentId, [FromBody] CommentEditModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            if (await this.comments.Update(forumId, postId, commentId, model) == false)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpDelete, Route("api/1.0/forums/{forumId}/posts/{postId}/comments/{commentId}")]
        public async Task<IActionResult> Remove(int forumId, int postId, int commentId)
        {
            if (await this.comments.Remove(forumId, postId, commentId) == false)
            {
                return this.NotFound();
            }

            return this.Ok();
        }
    }
}
