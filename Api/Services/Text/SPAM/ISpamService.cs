using Api.Models;

namespace Api.Services.Text.SPAM
{
    public interface ISpamService
    {
        Task<ClassificationResultModel[]> Classify(int forumId, string text);
        Task Train(int forumId, IEnumerable<ClassificationExampleModel> examples);
    }
}
