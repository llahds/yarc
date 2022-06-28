using Api.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Scoring
{
    public class UpdatePostScores : IUpdatePostScores
    {
        private readonly YARCContext context;

        public UpdatePostScores(
            YARCContext context)
        {
            this.context = context;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        public async Task Execute()
        {
            await this.context.Database.ExecuteSqlRawAsync("exec Social.UpdatePostScores");
        }
    }
}
