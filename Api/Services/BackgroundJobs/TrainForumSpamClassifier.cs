using Api.Data;
using Api.Models;
using Api.Services.Text.SPAM;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.BackgroundJobs
{
    public class TrainForumSpamClassifier
    {
        private readonly ISpamService spam;
        private readonly YARCContext context;

        public TrainForumSpamClassifier(
            ISpamService spam,
            YARCContext context)
        {
            this.spam = spam;
            this.context = context;
        }

        public async Task Train(int forumId)
        {
            var notSpam = await context
                .Posts
                .Where(P => P.ForumId == forumId && P.IsSpam == false)
                .OrderBy(G => Guid.NewGuid())
                .Take(1000)
                .Select(T => T.Title + "\r\n" + T.Text)
                .ToArrayAsync();

            var spam = await context
                .Posts
                .Where(P => P.ForumId == forumId && P.IsSpam == true)
                .OrderBy(G => Guid.NewGuid())
                .Take(1000)
                .Select(T => T.Title + "\r\n" + T.Text)
                .ToArrayAsync();

            var max = Math.Min(notSpam.Length, spam.Length);

            var examples = new List<ClassificationExampleModel>();

            for (var i = 0; i < max; i++)
            {
                examples.Add(new ClassificationExampleModel { Label = "not_spam", Text = notSpam[i] });
                examples.Add(new ClassificationExampleModel { Label = "spam", Text = spam[i] });
            }

            await this.spam.Train(forumId, examples);
        }
    }
}
