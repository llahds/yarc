namespace Api.Services.Text
{
    public class NaiveBayesClassifier
    {
        private readonly Dictionary<string, Category> _categories = new Dictionary<string, Category>();

        public IEnumerable<string> Categories()
        {
            return _categories.Keys;
        }

        public Category CategoryTokens(string category)
        {
            return _categories[category];
        }

        public NaiveBayesClassifier(Dictionary<string, Category> categories)
        {
            _categories = categories;
        }

        public NaiveBayesClassifier(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var c = 0; c < count; c++)
            {
                var categoryName = reader.ReadString();
                _categories.Add(categoryName, new Category(reader));
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this._categories.Count);
            foreach (var category in this._categories)
            {
                writer.Write(category.Key);
                category.Value.Write(writer);
            }
        }

        public CategoryResult[] Classify(string[] tokens)
        {
            if (_categories.Count == 1)
            {
                return new[] { new CategoryResult { Name = _categories.Keys.First(), Probability = 1 } };
            }

            var scores = new List<CategoryResult>();

            foreach (var category in _categories)
            {
                var likelihood = category.Value.Likelihood;
                foreach (var token in tokens)
                {
                    likelihood += category.Value.TokenWeight(token);
                }
                scores.Add(new CategoryResult { Name = category.Key, Probability = likelihood });
            }

            var total = scores.Sum(F => F.Probability);

            for (var r = 0; r < scores.Count; r++)
            {
                scores[r].Probability = scores[r].Probability / total;
            }

            var threshold = (scores.Max(S => S.Probability) - scores.Min(S => S.Probability)) / 2.0;

            return scores
                .Where(T => T.Probability >= threshold)
                .OrderByDescending(T => T.Probability)
                .ToArray();
        }
    }

    public class CategoryResult
    {
        public string Name { get; set; }
        public double Probability { get; set; }
    }

    public class Category
    {
        public double Likelihood { get; private set; }
        public double UnknownTokenLikelihood { get; private set; }

        private readonly Dictionary<string, TokenCount> _tokens = new Dictionary<string, TokenCount>();

        public Category(double likelihood, double unknownTokenLikelihood, Dictionary<string, TokenCount> tokens)
        {
            Likelihood = likelihood;
            UnknownTokenLikelihood = unknownTokenLikelihood;
            _tokens = tokens;
        }

        public IEnumerable<KeyValuePair<string, TokenCount>> Tokens()
        {
            return _tokens;
        }

        public Category(BinaryReader reader)
        {
            this.Likelihood = reader.ReadDouble();
            this.UnknownTokenLikelihood = reader.ReadDouble();
            var count = reader.ReadInt32();
            for (var c = 0; c < count; c++)
            {
                var token = reader.ReadString();
                var likelihood = reader.ReadDouble();

                _tokens.Add(token, new TokenCount { Likelihood = likelihood });
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.Likelihood);
            writer.Write(this.UnknownTokenLikelihood);
            writer.Write(this._tokens.Count);

            foreach (var kv in this._tokens)
            {
                writer.Write(kv.Key);
                writer.Write(kv.Value.Likelihood);
            }
        }

        public double TokenWeight(string token)
        {
            if (_tokens.ContainsKey(token))
            {
                return _tokens[token].Likelihood;
            }
            else
            {
                return 0;
                return UnknownTokenLikelihood;
            }
        }
    }

    public class TokenCount
    {
        public double Likelihood { get; set; }
    }

    public class NaiveBayesClassifierBuilder
    {
        private readonly Dictionary<string, int> _categoryCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, Dictionary<string, int>> _categoryTokens = new Dictionary<string, Dictionary<string, int>>();
        private readonly HashSet<string> _tokens = new HashSet<string>();

        public void Add(string categoryName, IEnumerable<string> tokens)
        {
            if (_categoryCounts.ContainsKey(categoryName) == false)
            {
                _categoryCounts.Add(categoryName, 0);
                _categoryTokens.Add(categoryName, new Dictionary<string, int>());
            }
            _categoryCounts[categoryName]++;

            foreach (var token in tokens)
            {
                if (!_categoryTokens[categoryName].ContainsKey(token))
                {
                    _categoryTokens[categoryName].Add(token, 0);
                }
                _categoryTokens[categoryName][token]++;
                _tokens.Add(token);
            }
        }

        public IEnumerable<KeyValuePair<string, int>> Categories()
        {
            return this._categoryCounts;
        }

        public IEnumerable<KeyValuePair<string, int>> CategoryTokens(string category)
        {
            return _categoryTokens[category];
        }

        public NaiveBayesClassifier Build()
        {
            var totalDocuments = (double)this._categoryCounts.Sum(K => K.Value);

            var categoryProbability = this._categoryCounts
                .Select(K => new
                {
                    Category = K.Key,
                    Probability = Math.Log((double)K.Value / (double)totalDocuments)
                })
                .ToDictionary(K => K.Category, V => V.Probability);

            var categories = new Dictionary<string, Category>();

            foreach (var kv in _categoryTokens)
            {
                var totalCategoryTerms = (double)kv.Value.Sum(T => T.Value);

                var terms = new Dictionary<string, double>();

                var unknownTokenLikelihood = Math.Log(1.0 / (totalCategoryTerms + this._tokens.Count));

                foreach (var term in _tokens)
                {
                    if (kv.Value.ContainsKey(term))
                    {
                        terms.Add(term, Math.Log((kv.Value[term] + 1.0) / (totalCategoryTerms + this._tokens.Count)));
                    }
                }

                categories.Add(kv.Key, new Category(
                    likelihood: categoryProbability[kv.Key],
                    unknownTokenLikelihood: unknownTokenLikelihood,
                    tokens: terms.ToDictionary(K => K.Key, V => new TokenCount { Likelihood = V.Value })
                ));
            }

            return new NaiveBayesClassifier(
                categories: categories
            );
        }
    }
}
