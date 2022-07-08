using Api.Data;
using Api.Services.Reporting;
using Api.Services.Text.Toxicity;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.BackgroundJobs
{
    public class CheckPostToxicity : ICheckPostToxicity
    {
        private readonly IToxicityService toxicity;
        private readonly YARCContext context;
        private readonly IReportingService reporting;

        public CheckPostToxicity(
            IToxicityService toxicity,
            YARCContext context,
            IReportingService reporting)
        {
            this.toxicity = toxicity;
            this.context = context;
            this.reporting = reporting;
        }

        public async Task Check(int postId)
        {
            var post = await this.context.Posts
                .FirstOrDefaultAsync(F => F.Id == postId);

            var text = $"{post?.Title ?? ""}\r\n{post?.Text ?? ""}";

            var result = this.toxicity.Classify(text);

            // TODO: The probability score threshold needs to be stored with the forum settings
            if (result[0].Label == "toxic" && result[0].Probability > .75)
            {
                await this.reporting.CreateFromPost(new Models.ReportedPostModel
                {
                    PostId = postId,
                    ReasonId = -1
                });
            }
        }
    }
}
