using System.Text.RegularExpressions;
using YARC.DL;
using YARC.DL.Training;

namespace Api.Services.Text.Toxicity
{
    public class ToxicityModel
    {
        private readonly Dictionary<string, int> labels;
        private readonly Embedding tokens;
        private readonly Lstm forward;
        private readonly Linear linear;
        private readonly Softmax softmax;

        public ToxicityModel(
            Dictionary<string, int> labels,
            Embedding tokens,
            Lstm forward,
            Linear linear,
            Softmax softmax
            )
        {
            this.labels = labels;
            this.tokens = tokens;
            this.forward = forward;
            this.linear = linear;
            this.softmax = softmax;
        }

        public Label[] Predict(string[] sequence)
        {
            tokens.ResetState();
            forward.ResetState();
            linear.ResetState();
            softmax.ResetState();

            var graph = new Graph();

            var forwardOutput = new List<Matrix>();
            foreach (var token in sequence)
            {
                var tokenInput = tokens.Activate(token.ToLower(), graph);
                var output = forward.Activate(tokenInput, graph);
                forwardOutput.Add(output);
            }

            Matrix linearOutput = forwardOutput.Last();

            var o = softmax.Activate(linear.Activate(linearOutput, graph), graph);

            return o.W
                .Select((S, I) => new Label
                {
                    Name = labels.Keys.ElementAt(I),
                    Score = S
                })
                .OrderByDescending(S => S.Score)
                .ToArray();
        }

        public void Train(double learningRate, int epochs, Func<IEnumerable<Example>> trainingData)
        {
            var loss = new LossSumOfSquares();

            for (var epoch = 0; epoch < epochs; epoch++)
            {
                var sequenceCount = 0;

                foreach (var sequence in trainingData())
                {
                    sequenceCount++;

                    if (sequenceCount % 100 == 0)
                    {
                        Console.WriteLine($"Epoch: {epoch}, Sequence: {sequenceCount}");
                    }

                    tokens.ResetState();
                    forward.ResetState();
                    linear.ResetState();
                    softmax.ResetState();

                    var graph = new TrainingGraph();

                    var forwardOutput = new List<Matrix>();
                    foreach (var token in sequence.Tokens)
                    {
                        var tokenInput = tokens.Activate(token.ToLower(), graph);
                        var output = forward.Activate(tokenInput, graph);
                        forwardOutput.Add(output);
                    }

                    Matrix linearOutput = forwardOutput.Last();

                    var o = softmax.Activate(linear.Activate(linearOutput, graph), graph);

                    var target = new double[labels.Count];
                    target[labels[sequence.Label]] = 1;

                    loss.Backward(o, new Matrix(target));

                    graph.Backward();

                    Trainer.UpdateModelParams(learningRate, tokens, forward, linear, softmax);
                }
            }
        }

        public static ToxicityModel Build(Func<IEnumerable<Example>> trainingData)
        {
            var labels = trainingData()
                .Select(S => S.Label)
                .Distinct()
                .Select((S, I) => new { Label = S ?? "UNK", Index = I })
                .ToDictionary(K => K.Label, V => V.Index);

            if (labels.ContainsKey("UNK") == false)
            {
                labels.Add("UNK", labels.Count);
            }

            var tokenMap = new HashSet<string>(trainingData().SelectMany(S => S.Tokens.Select(M => M.ToLower())));

            tokenMap.Add("<UNK>");

            var random = new Random();
            var tokens = new Embedding(tokenMap, 8, .1, random);
            var forward = new Lstm(8, 16, .1, random);
            var linear = new Linear(16, labels.Count, .1, random);
            var softmax = new Softmax();

            var labeller = new ToxicityModel(
                labels: labels,
                tokens: tokens,
                forward: forward,
                linear: linear,
                softmax: softmax
            );

            return labeller;
        }

        public void Save(Stream stream)
        {
            using (stream)
            {
                using (var writer = new BinaryWriter(stream))
                {
                    this.Save(writer);
                }
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(labels.Count);
            foreach (var kv in labels)
            {
                writer.Write(kv.Key);
                writer.Write(kv.Value);
            }

            tokens.Save(writer);
            forward.Save(writer);
            linear.Save(writer);
        }

        public static ToxicityModel Load(Stream stream)
        {
            using (stream)
            {
                using (var reader = new BinaryReader(stream))
                {
                    return Load(reader);
                }
            }
        }

        public static ToxicityModel Load(BinaryReader reader)
        {
            var labels = new Dictionary<string, int>();
            var labelCount = reader.ReadInt32();
            for (var i = 0; i < labelCount; i++)
            {
                labels.Add(reader.ReadString(), reader.ReadInt32());
            }

            var tokens = Embedding.Load(reader);
            var forward = Lstm.Load(reader);
            var linear = Linear.Load(reader);
            var softmax = new Softmax();

            return new ToxicityModel(
                labels: labels,
                tokens: tokens,
                forward: forward,
                linear: linear,
                softmax: softmax
            );
        }

        public class Label
        {
            public string Name { get; set; }
            public double Score { get; set; }

            public override string ToString()
            {
                return $"{Name}: {Score.ToString("N2")}";
            }
        }

        public class Example
        {
            public string Label { get; set; }
            public string[] Tokens { get; set; }
        }
    }
}
