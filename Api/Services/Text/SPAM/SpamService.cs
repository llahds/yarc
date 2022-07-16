using Api.Models;
using Api.Services.IO.Blobs;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;

namespace Api.Services.Text.SPAM
{
    public class SpamService : ISpamService, IDisposable
    {
        private readonly IMemoryCache memoryCache;
        private readonly IBlobService blobs;

        public SpamService(
            IMemoryCache memoryCache,
            IBlobService blobs)
        {
            this.memoryCache = memoryCache;
            this.blobs = blobs;
        }

        public async Task<ClassificationResultModel[]> Classify(int forumId, string text)
        {
            var classifier = await this.memoryCache.GetOrCreateAsync($"SPAM_MODEL_{forumId}", async entry =>
            {
                entry.Size = 1;
                entry.SlidingExpiration = new TimeSpan(1, 0, 0);
                if (await this.blobs.Exists("FORUM_SPAM_MODELS", forumId.ToString()))
                {
                    using (var reader = new BinaryReader(await this.blobs.GetBlob("FORUM_SPAM_MODELS", forumId.ToString())))
                    {
                        return new NaiveBayesClassifier(reader);
                    }
                }
                else
                {
                    return null;
                }
            });

            var result = classifier?.Classify(this.tokenize(text).ToArray())
                ?? new CategoryResult[0];

            return result
                .Select(R => new ClassificationResultModel
                {
                    Label = R.Name,
                    Probability = R.Probability
                })
                .ToArray();
        }

        public void Dispose()
        {
            this.memoryCache.Dispose();
        }

        public async Task Train(int forumId, IEnumerable<ClassificationExampleModel> examples)
        {
            var builder = new NaiveBayesClassifierBuilder();
            
            foreach (var example in examples)
            {
                builder.Add(example.Label, this.tokenize(example.Text));
            }

            var classifier = builder.Build();

            using (var ms = new MemoryStream())
            {
                using (var w = new BinaryWriter(ms))
                {
                    classifier.Write(w);

                    ms.Seek(0, SeekOrigin.Begin);

                    await this.blobs.UploadBlob("FORUM_SPAM_MODELS", forumId.ToString(), ms, true);
                }
            }

            this.memoryCache.Set($"SPAM_MODEL_{forumId}", classifier, new MemoryCacheEntryOptions
            {
                SlidingExpiration = new TimeSpan(1, 0, 0)
            });
        }

        private IEnumerable<string> tokenize(string text)
        {
            return Regex.Split(text ?? "", @"\W+");
        }
    }
}
