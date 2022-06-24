using Api.Models;

namespace Api.Services.Posts
{
    public interface IPostService
    {
        Task<IdModel<int>> Create(int forumId, EditForumPostModel model);
        Task<ForumPostListItemModel> Get(int forumId, int postId);
        Task<ForumPostListItemModel[]> List(int forumId, int startAt);
        Task<bool> Remove(int forumId, int postId);
        Task<bool> Update(int forumId, int postId, EditForumPostModel model);
        Task VoteDown(int forumId, int postId);
        Task VoteUp(int forumId, int postId);
    }
}
