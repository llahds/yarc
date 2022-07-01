using Api.Common;
using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Posts
{
    public class PostValidationService : IPostValidationService
    {
        private readonly YARCContext context;

        public PostValidationService(
            YARCContext context)
        {
            this.context = context;
        }

        public async Task<ValidationErrorModel> ValidatePost(int forumId, EditForumPostModel model)
        {
            var settings = await this.context
                .Forums
                .Where(F => F.Id == forumId)
                .Select(S => S.PostSettings)
                .FirstOrDefaultAsync();

            if (settings?.RequiredTitleWords?.Length > 0
                && model.Title.Contains(settings.RequiredTitleWords) == false)
            {
                return new ValidationErrorModel
                {
                    Field = "Title",
                    Message = "Post title is missing words required by the moderators."
                };
            }

            if (settings?.BannedTitleWords?.Length > 0
                && model.Title.Contains(settings.BannedTitleWords))
            {
                return new ValidationErrorModel
                {
                    Field = "Title",
                    Message = "Post title contains words banned by the moderators."
                };
            }

            if (settings?.PostTextBannedWords?.Length > 0
                && model.Text.Contains(settings.PostTextBannedWords))
            {
                return new ValidationErrorModel
                {
                    Field = "Text",
                    Message = "Post text contains words banned by the moderators."
                };
            }

            return null;
        }
    }


}
