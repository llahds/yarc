using Api.Data;
using Api.Services.Reporting;
using Api.Services.Text.SPAM;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.BackgroundJobs
{

    public class CheckPostSpam 
    {
        private readonly YARCContext context;
        private readonly IReportingService reporting;
        private readonly ISpamService spam;

        public CheckPostSpam(
            YARCContext context,
            IReportingService reporting,
            ISpamService spam)
        {
            this.context = context;
            this.reporting = reporting;
            this.spam = spam;
        }

        public async Task Check(int postId)
        {
            var post = await this.context.Posts
                .FirstOrDefaultAsync(F => F.Id == postId);

            var text = $"{post?.Title ?? ""}\r\n{post?.Text ?? ""}";

            var result = await this.spam.Classify(post.ForumId, text);

            // TODO: The probability score threshold needs to be stored with the forum settings
            if (result.Length > 0 && result[0].Label == "spam" && result[0].Probability > .75)
            {
                await this.reporting.CreateFromPost(new Models.ReportedPostModel
                {
                    PostId = postId,
                    ReasonId = 13
                });
            }
        }
    }
}
