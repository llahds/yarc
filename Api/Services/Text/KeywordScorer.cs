using System.Text.RegularExpressions;

namespace Api.Services.Text
{
    public class KeywordScorer
    {
        private readonly Dictionary<string, int> _categoryCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, Dictionary<string, int>> _categoryTokens = new Dictionary<string, Dictionary<string, int>>();
        private readonly HashSet<string> _tokens = new HashSet<string>();

        public KeywordScorer()
        {
        }

        public IEnumerable<string> Tokenize(string text)
        {
            return Regex.Split(text, @"(\w+)").Select(T => T.ToLower()).Select(T => T);
        }

        public void Add(string categoryName, string content)
        {
            if (_categoryCounts.ContainsKey(categoryName) == false)
            {
                _categoryCounts.Add(categoryName, 0);
                _categoryTokens.Add(categoryName, new Dictionary<string, int>());
            }
            _categoryCounts[categoryName]++;

            foreach (var token in this.Tokenize(content).Distinct())
            {
                if (Regex.IsMatch(token, @"[^a-z0-9\#\+\.]") == false)
                {
                    if (!_categoryTokens[categoryName].ContainsKey(token))
                    {
                        _categoryTokens[categoryName].Add(token, 0);
                    }
                    _categoryTokens[categoryName][token]++;
                    _tokens.Add(token);
                }
            }
        }

        public IEnumerable<Topic> Extract(double factor)
        {
            var tokenWeights = _tokens.Select(token => new
            {
                token = token,
                count = _categoryTokens.Count(KV => KV.Value.ContainsKey(token))
            })
            .Select(counts => new
            {
                token = counts.token,
                weight = Math.Log((double)_categoryCounts.Count / (double)counts.count)
            })
            .ToDictionary(key => key.token, key => key.weight);

            foreach (var category in _categoryTokens.Keys)
            {
                var scores = new List<Tuple<string, double>>();
                foreach (var token in _categoryTokens[category].Keys)
                {
                    var localScore = (double)_categoryTokens[category][token] / (double)_categoryCounts[category];
                    var categoryCount = (double)_categoryTokens.Count(KV => KV.Value.ContainsKey(token));
                    var clusterScore = 1 - categoryCount / (double)_categoryTokens.Count;
                    var score = ((localScore * localScore) * clusterScore);
                    score = score * tokenWeights[token];
                    if (score > 0)
                    {
                        scores.Add(new Tuple<string, double>(token, score));
                    }
                }

                if (scores.Count() > 0)
                {
                    var max = scores.Max(K => K.Item2);
                    var min = scores.Min(K => K.Item2) - 0.0000001;

                    var mean = max / factor;

                    var topN = scores.Where(T => T.Item2 > mean)
                        .OrderByDescending(T => T.Item2)
                        .Select(T => new Keyword(T.Item1, T.Item2))
                        .ToList();

                    for (var i = 0; i < topN.Count; i++)
                    {
                        topN[i].Score = ((topN[i].Score - min) / (max - min)) * (1.0 - 0.000001) + 0.000001;
                    }

                    yield return new Topic(category, topN.ToArray());
                }
            }
        }

        public class Topic
        {
            public string Name { get; private set; }
            public Keyword[] Keywords { get; private set; }

            public Topic(string name, Keyword[] keywords)
            {
                this.Name = name;
                this.Keywords = keywords;
            }

            public override string ToString()
            {
                return this.Name;
            }
        }

        public class Keyword
        {
            public string Token { get; private set; }
            public double Score { get; set; }

            public Keyword(string token, double score)
            {
                this.Token = token;
                this.Score = score;
            }

            public override string ToString()
            {
                return this.Token + " - " + this.Score.ToString("N3");
            }
        }
    }
}
