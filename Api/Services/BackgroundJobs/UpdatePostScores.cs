using Api.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.BackgroundJobs
{
    public class UpdatePostScores
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
            await context.Database.ExecuteSqlRawAsync("exec Social.UpdatePostScores");
        }
    }
}
