using Api.Models;

namespace Api.Services.Posts
{
    public interface IPostViewService
    {
        Task<ForumPostListItemModel[]> Popular(int startAt);
    }
}
