using Api.Models;

namespace Api.Services.Moderation
{
    public interface IModerationService
    {
        Task ApproveComment(int forumId, int commentId);
        Task ApprovePost(int forumId, int postId);
        Task ChangeForumMemberStatus(int forumId, int userId, ForumMemberStatusModel model);
        Task<ForumPostSettingsModel> GetForumPostSettings(int forumId);
        Task<ReportQueueItemModel[]> GetModerationQueue(int forumId, int startAt, int[] reasonIds);
        Task<bool> IsModerator(int forumId);
        Task<bool> IsOwner(int forumId);
        Task RejectComment(int forumId, int commentId);
        Task RejectPost(int forumId, int postId);
        Task RemoveForumMember(int forumId, int userId);
        Task<UserInfoModel[]> SearchForumMembers(int forumId, int startAt, string query, int status);
        Task<UserInfoModel[]> SearchUsers(string query);
        Task UpdateForumPostSettings(int forumId, ForumPostSettingsModel model);
    }
}