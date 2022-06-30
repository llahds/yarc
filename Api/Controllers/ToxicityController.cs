using Api.Models;
using Api.Services.Text.Toxicity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ToxicityController : Controller
    {
        private readonly IToxicityService toxicity;

        public ToxicityController(
            IToxicityService toxicity)
        {
            this.toxicity = toxicity;
        }

        [HttpPost, Route("api/1.0/classification/toxicity")]
        [ProducesResponseType(200, Type = typeof(ClassificationResultModel[]))]
        public async Task<IActionResult> Classify([FromBody] ClassifyTextRequestModel model)
        {
            if (this.ModelState.IsValid == false)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(this.toxicity.Classify(model.Text));
        }
    }
}
