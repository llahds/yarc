namespace Api.Services.Moderation
{
    public interface IModerationService
    {
        Task<bool> IsModerator(int forumId);
        Task<bool> IsOwner(int forumId);
    }
}