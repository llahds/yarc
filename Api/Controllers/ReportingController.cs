using Api.Models;
using Api.Services.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        private readonly IReportingService reporting;

        public ReportingController(
            IReportingService reporting)
        {
            this.reporting = reporting;
        }

        [AllowAnonymous]
        [HttpGet, Route("api/1.0/reporting/reasons")]
        [ProducesResponseType(200, Type = typeof(ReportReasonModel[]))]
        public async Task<IActionResult> GetReportingReasons()
        {
            return this.Ok(await this.reporting.GetReportReasons());
        }

        [Authorize]
        [HttpPost, Route("api/1.0/reporting/posts")]
        public async Task<IActionResult> CreateFromPost([FromBody] ReportedPostModel model)
        {
            await this.reporting.CreateFromPost(model);

            return this.Ok();
        }

        [Authorize]
        [HttpPost, Route("api/1.0/reporting/comments")]
        public async Task<IActionResult> CreateFromComment([FromBody] ReportedCommentModel model)
        {
            await this.reporting.CreateFromComment(model);

            return this.Ok();
        }
    }
}
