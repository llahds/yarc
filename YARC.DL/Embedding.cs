namespace YARC.DL
{
    // this code was ported from https://github.com/mashmawy/Seq2SeqLearn
    public class Embedding : ILayer
    {
        private readonly Matrix _w;
        private readonly Dictionary<string, int> _tokens;

        public Embedding(Matrix w, Dictionary<string, int> tokens)
        {
            _w = w;
            _tokens = tokens;
        }

        public int IndexOf(string token)
        {
            if (_tokens.ContainsKey(token))
            {
                return _tokens[token];
            }
            else
            {
                return -1;
            }
        }

        public string IndexOf(int index)
        {
            return _tokens.Keys.ElementAt(index);
        }

        public int Count()
        {
            return _tokens.Count;
        }

        public Embedding(HashSet<string> tokens, int outputDimension, double initParamsStdDev, Random rng)
        {
            if (tokens.Contains("<UNK>") == false)
            {
                tokens.Add("<UNK>");
            }

            _tokens = tokens
                .Select((T, I) => new { Token = T, Index = I })
                .ToDictionary(K => K.Token, V => V.Index);

            _w = Matrix.Random(tokens.Count + 1, outputDimension, initParamsStdDev, rng);
        }

        public Matrix Activate(string token, Graph g)
        {
            var index = _tokens["<UNK>"];

            if (_tokens.ContainsKey(token))
            {
                index = _tokens[token];
            }

            return g.Peek(_w, index);
        }

        //public int IndexOf(Matrix matrix, Graph g)
        //{
        //    var relu = new RectifiedLinearUnit();
        //    var w = g.Nonlin(relu, g.Mul(_w, matrix));
        //    //var w = g.Softmax(s);

        //    var best = w.W
        //        .Select((S, I) => new
        //        {
        //            Index = I,
        //            Score = S
        //        })
        //        .OrderByDescending(S => S.Score)
        //        .ToArray();

        //    return -1;
        //}

        public Matrix Activate(Matrix m, Graph g)
        {
            return m;
        }

        public IEnumerable<string> Tokens()
        {
            return this._tokens.Keys;
        }

        public List<Matrix> GetParameters()
        {
            return new List<Matrix>
            {
                _w
            };
        }

        public void ResetState()
        {
            
        }

        public void Save(BinaryWriter writer)
        {
            _w.Save(writer);

            writer.Write(_tokens.Count);
            foreach (var kv in _tokens)
            {
                writer.Write(kv.Key);
                writer.Write(kv.Value);
            }
        }

        public static Embedding Load(BinaryReader reader)
        {
            var w = Matrix.Load(reader);

            var tokens = new Dictionary<string, int>();
            var tokenCount = reader.ReadInt32();
            for (var i = 0; i < tokenCount; i++)
            {
                tokens.Add(reader.ReadString(), reader.ReadInt32());
            }

            return new Embedding(w, tokens);
        }
    }

    //public static class Extensions
    //{
    //    public static double Cosine(this double[] v1, double[] v2)
    //    {
    //        var cosine = v1.Inner(v2) / (v1.Norm() * v2.Norm());
    //        if (double.IsNaN(cosine))
    //        {
    //            cosine = 0.0;
    //        }
    //        return cosine;
    //    }

    //    public static double Inner(this double[] v1, double[] v2)
    //    {
    //        double r = 0;
    //        for (int i = 0; i < v1.Length; i++)
    //        {
    //            r += v1[i] * v2[i];
    //        }
    //        return (float)r;
    //    }

    //    public static double Norm(this double[] v1)
    //    {
    //        double r = 0;
    //        for (int i = 0; i < v1.Length; i++)
    //        {
    //            r += System.Math.Pow(v1[i], 2);
    //        }
    //        r = System.Math.Pow(r, 0.5);
    //        return (float)r;
    //    }

    //}
}
