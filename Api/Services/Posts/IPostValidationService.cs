using Api.Models;

namespace Api.Services.Posts
{
    public interface IPostValidationService
    {
        Task<ValidationErrorModel> ValidatePost(int forumId, EditForumPostModel model);
    }
}