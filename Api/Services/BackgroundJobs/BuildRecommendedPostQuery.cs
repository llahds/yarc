using Api.Data;
using Api.Services.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.Services.BackgroundJobs
{
    public class BuildRecommendedPostQuery
    {
        private readonly YARCContext context;

        public BuildRecommendedPostQuery(
            YARCContext context)
        {
            this.context = context;
        }

        public async Task Build(string userName)
        {
            var ups = await this.context
                .PostVotes
                .Where(PV => PV.By.UserName == userName && PV.Vote == 1 && PV.Post.IsDeleted == false && PV.Post.IsHidden == false)
                .OrderByDescending(PV => PV.CreatedOn)
                .Take(200)
                .Select(P => P.Post)
                .ToArrayAsync();

            var downs = await this.context
                .PostVotes
                .Where(PV => PV.By.UserName == userName && PV.Vote == -1 && PV.Post.IsDeleted == false && PV.Post.IsHidden == false)
                .OrderByDescending(PV => PV.CreatedOn)
                .Take(200)
                .Select(P => P.Post)
                .ToArrayAsync();

            var keywords = new KeywordScorer();

            foreach (var up in ups)
            {
                keywords.Add("up", $"{up.Title}\r\n{up.Text}");
            }

            foreach (var down in downs)
            {
                keywords.Add("down", $"{down.Title}\r\n{down.Text}");
            }

            var scores = keywords.Extract(10.0).ToArray();

            var setting = await this.context
                .UserSettings
                .Where(U => U.Key == "RECOMMEND_POST_QUERY" && U.User.UserName == userName)
                .FirstOrDefaultAsync();

            if (setting == null)
            {
                setting = new Data.Entities.UserSetting
                {
                    UserId = this.context.Users
                        .Where(U => U.UserName == userName)
                        .Select(T => T.Id)
                        .First(),
                    Key = "RECOMMEND_POST_QUERY"
                };

                await this.context.AddAsync(setting);
            }

            setting.Value = JsonConvert.SerializeObject(scores);

            await this.context.SaveChangesAsync();
        }
    }
}
