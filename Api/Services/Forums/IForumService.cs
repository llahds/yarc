using Api.Models;

namespace Api.Services.Forums
{
    public interface IForumService
    {
        Task<bool> CanAccessForum(int forumId);
        Task<IdModel<int>> Create(EditForumModel model);
        Task<SimilarForumModel[]> FindSimilarForums(int forumId);
        Task<ForumModel> Get(int forumId);
        Task<ForumPostGuideLinesModel> GetForumGuidelines(int forumId);
        Task<bool> Remove(int forumId);
        Task<bool> SlugAlreadyTaken(string slug, int? forumId);
        Task<KeyValueModel[]> SuggestTopics(string queryText);
        Task<KeyValueModel[]> SuggestUsers(string queryText);
        Task<bool> Update(int forumId, EditForumModel model);
        Task<bool> VerifyOwner(int forumId);
        Task<bool> VerifyModerator(int forumId);
        Task Join(int forumId);
        Task Leave(int forumId);
        Task<int?> GetMemberStatus(int forumId);
    }
}
