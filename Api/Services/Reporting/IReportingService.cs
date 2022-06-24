using Api.Models;

namespace Api.Services.Reporting
{
    public interface IReportingService
    {
        Task CreateFromComment(ReportedCommentModel model);
        Task CreateFromPost(ReportedPostModel model);
        Task<ReportReasonModel[]> GetReportReasons();
    }
}
