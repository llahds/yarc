using Api.Models;

namespace Api.Services.Text.Toxicity
{
    public interface IToxicityService
    {
        ClassificationResultModel[] Classify(string text);
    }
}