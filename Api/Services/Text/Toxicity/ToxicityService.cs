using Api.Models;
using System.Text.RegularExpressions;

namespace Api.Services.Text.Toxicity
{
    public class ToxicityService : IToxicityService
    {
        private readonly ToxicityModel model;
        private readonly HashSet<string> stopwords;

        public ToxicityService()
        {
            this.stopwords = File.ReadAllLines("./Services/Text/stopwords.txt").ToHashSet();
            this.model = ToxicityModel.Load(File.OpenRead("./Services/Text/Toxicity/toxicity.model"));
        }

        public ClassificationResultModel[] Classify(string text)
        {
            var tokens = this.Tokenize(text);

            if (tokens.Length == 0)
            {
                return new ClassificationResultModel[0];
            }

            return this.model.Predict(tokens)
                .Select(C => new ClassificationResultModel
                {
                    Label = C.Name,
                    Probability = C.Score
                })
                .ToArray();
        }

        private string[] Tokenize(string text)
        {
            return Regex.Split(text.ToLower(), @"\W+")
                .Where(R => Regex.IsMatch(R, @"[^a-z]") == false && R.Length > 2 && stopwords.Contains(R) == false)
                .Take(50)
                .ToArray();
        }
    }
}
