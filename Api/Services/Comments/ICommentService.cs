using Api.Models;

namespace Api.Services.Comments
{
    public interface ICommentService
    {
        Task<bool> CanEditComment(int forumId, int postId, int commentId);
        Task<CommentModel> CreateAtParent(int forumId, int postId, int parentId, CommentEditModel model);
        Task<CommentModel> CreateAtRoot(int forumId, int postId, CommentEditModel model);
        Task<CommentModel[]> GetComments(int forumId, int postId, int? parentId);
        Task<bool> Remove(int forumId, int postId, int commentId);
        Task<bool> Update(int forumId, int postId, int commentId, CommentEditModel model);
        Task VoteDown(int forumId, int postId, int commentId);
        Task VoteUp(int forumId, int postId, int commentId);
    }
}
