namespace YARC.DL
{
    public class Feature : Linear
    {
        private readonly Dictionary<string, int> features;

        public Feature(Dictionary<string, int> features,
            Matrix w)
            : base(w)
        {
            this.features = features;
        }

        public Feature(
            Dictionary<string, int> features,
            int outputSize,
            double standardDev,
            Random random)
            : base(features.Count, outputSize, standardDev, random)
        {
            this.features = features;
        }

        public Matrix Activate(IEnumerable<string> f, Graph g)
        {
            var v = new double[features.Count];
            foreach (var feature in f)
            {
                if (features.ContainsKey(feature))
                {
                    v[features[feature]] = 1;
                }
            }
            return base.Activate(new Matrix(v), g);
        }

        public void Save(BinaryWriter writer)
        {
            foreach (var w in base.GetParameters())
            {
                w.Save(writer);
            }

            writer.Write(features.Count);
            foreach (var kv in features)
            {
                writer.Write(kv.Key);
                writer.Write(kv.Value);
            }
        }

        public static Feature Load(BinaryReader reader)
        {
            var w = Matrix.Load(reader);

            var features = new Dictionary<string, int>();
            var featureCount = reader.ReadInt32();
            for (var i = 0; i < featureCount; i++)
            {
                features.Add(reader.ReadString(), reader.ReadInt32());
            }

            return new Feature(features, w);
        }
    }
}
