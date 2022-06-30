using Api.Models;

namespace Api.Services.Posts
{
    public interface IPostViewService
    {
        Task<ListResultModel<ForumPostListItemModel>> Popular(int startAt, string sort);
    }
}
