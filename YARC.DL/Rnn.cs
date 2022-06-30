namespace YARC.DL
{
    public class Rnn : ILayer
    {
        private readonly Matrix _w;
        private readonly Matrix _b;

        public Matrix _context;

        private readonly INonlinearity _f;

        public Rnn(Matrix w, Matrix b, INonlinearity f)
        {
            _w = w;
            _b = b;
            _f = f;
        }

        public Rnn(int inputDimension, int outputDimension, INonlinearity hiddenUnit, double initParamsStdDev,
            Random rng)
        {
            this._f = hiddenUnit;
            _w = Matrix.Random(outputDimension, inputDimension + outputDimension, initParamsStdDev, rng);
            _b = new Matrix(outputDimension);
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            Matrix concat = g.ConcatVectors(input, _context);
            Matrix sum = g.Mul(_w, concat); sum = g.Add(sum, _b);
            Matrix output = g.Nonlin(_f, sum);

            //rollover activations for next iteration
            _context = output;

            return output;
        }

        public double[] Context()
        {
            return _context.W;
        }

        public void ResetState()
        {
            _context = new Matrix(_w.Rows);
        }

        public void ResetState(Rnn rnn)
        {
            _context = rnn._context;
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

        public static Rnn Load(BinaryReader reader)
        {
            var w = Matrix.Load(reader);
            var b = Matrix.Load(reader);
            var ft = Type.GetType(reader.ReadString());
            var f = (INonlinearity)Activator.CreateInstance(ft);
            return new Rnn(w, b, f);
        }
    }
}
