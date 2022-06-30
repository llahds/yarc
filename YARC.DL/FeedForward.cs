namespace YARC.DL
{
    public class FeedForward : ILayer
    {
        readonly Matrix _w;
        readonly Matrix _b;
        readonly INonlinearity _f;

        public FeedForward(Matrix w, Matrix b, INonlinearity f)
        {
            _w = w;
            _b = b;
            _f = f;
        }

        public FeedForward(int inputDimension, int outputDimension, INonlinearity f, double initParamsStdDev, Random rng)
        {
            _w = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _b = new Matrix(outputDimension);
            this._f = f;
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            Matrix sum = g.Add(g.Mul(_w, input), _b);
            Matrix returnObj = g.Nonlin(_f, sum);
            return returnObj;
        }

        public void ResetState()
        {

        }

        public List<Matrix> GetParameters()
        {
            List<Matrix> result = new List<Matrix>();
            result.Add(_w);
            result.Add(_b);
            return result;
        }

        public void Save(BinaryWriter writer)
        {
            _w.Save(writer);
            _b.Save(writer);
            writer.Write(_f.GetType().ToString());
        }

        public static FeedForward Load(BinaryReader reader)
        {
            var w = Matrix.Load(reader);
            var b = Matrix.Load(reader);
            var ft = Type.GetType(reader.ReadString());
            var f = (INonlinearity)Activator.CreateInstance(ft);
            return new FeedForward(w, b, f);
        }
    }
}
