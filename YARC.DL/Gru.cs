namespace YARC.DL
{
    public class Gru : ILayer
    {
        private readonly int _inputDimension;
        private readonly int _outputDimension;

        private readonly Matrix _hmix;
        private readonly Matrix _hHmix;
        private readonly Matrix _bmix;
        private readonly Matrix _hnew;
        private readonly Matrix _hHnew;
        private readonly Matrix _bnew;
        private readonly Matrix _hreset;
        private readonly Matrix _hHreset;
        private readonly Matrix _breset;

        private Matrix _context;

        private readonly INonlinearity _fMix = new SigmoidUnit();
        private readonly INonlinearity _fReset = new SigmoidUnit();
        private readonly INonlinearity _fNew = new TanhUnit();

        public Gru(
            int inputDimension,
            int outputDimension,
            Matrix hmix,
            Matrix hHmix,
            Matrix bmix,
            Matrix hnew,
            Matrix hHnew,
            Matrix bnew,
            Matrix hreset,
            Matrix hHreset,
            Matrix breset)
        {
            _inputDimension = inputDimension;
            _outputDimension = outputDimension;
            _hmix = hmix;
            _hHmix = hHmix;
            _bmix = bmix;
            _hnew = hnew;
            _hHnew = hHnew;
            _bnew = bnew;
            _hreset = hreset;
            _hHreset = hHreset;
            _breset = breset;
        }

        public Gru(int inputDimension, int outputDimension, double initParamsStdDev, Random rng)
        {
            this._inputDimension = inputDimension;
            this._outputDimension = outputDimension;

            _hmix = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _hHmix = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            _bmix = new Matrix(outputDimension);

            _hnew = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _hHnew = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            _bnew = new Matrix(outputDimension);

            _hreset = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _hHreset = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            _breset = new Matrix(outputDimension);
        }

        public Matrix Activate(Matrix input, Graph g)
        {

            Matrix sum0 = g.Mul(_hmix, input);
            Matrix sum1 = g.Mul(_hHmix, _context);
            Matrix actMix = g.Nonlin(_fMix, g.Add(g.Add(sum0, sum1), _bmix));

            Matrix sum2 = g.Mul(_hreset, input);
            Matrix sum3 = g.Mul(_hHreset, _context);
            Matrix actReset = g.Nonlin(_fReset, g.Add(g.Add(sum2, sum3), _breset));

            Matrix sum4 = g.Mul(_hnew, input);
            Matrix gatedContext = g.Elmul(actReset, _context);
            Matrix sum5 = g.Mul(_hHnew, gatedContext);
            Matrix actNewPlusGatedContext = g.Nonlin(_fNew, g.Add(g.Add(sum4, sum5), _bnew));

            Matrix memvals = g.Elmul(actMix, _context);
            Matrix newvals = g.Elmul(g.OneMinus(actMix), actNewPlusGatedContext);
            Matrix output = g.Add(memvals, newvals);

            //rollover activations for next iteration
            _context = output;

            return output;
        }

        public void ResetState()
        {
            _context = new Matrix(_outputDimension);
        }

        public void ResetState(Gru gru)
        {
            _context = gru._context;
        }

        public List<Matrix> GetParameters()
        {
            List<Matrix> result = new List<Matrix>();
            result.Add(_hmix);
            result.Add(_hHmix);
            result.Add(_bmix);
            result.Add(_hnew);
            result.Add(_hHnew);
            result.Add(_bnew);
            result.Add(_hreset);
            result.Add(_hHreset);
            result.Add(_breset);
            return result;
        }

        public static Gru Load(BinaryReader reader)
        {
            var inputDimension = reader.ReadInt32();
            var outputDimension = reader.ReadInt32();

            var hmix = Matrix.Load(reader);
            var hHmix = Matrix.Load(reader);
            var bmix = Matrix.Load(reader);
            var hnew = Matrix.Load(reader);
            var hHnew = Matrix.Load(reader);
            var bnew = Matrix.Load(reader);
            var hreset = Matrix.Load(reader);
            var hHreset = Matrix.Load(reader);
            var breset = Matrix.Load(reader);

            return new Gru(
                inputDimension: inputDimension,
                outputDimension: outputDimension,
                hmix: hmix,
                hHmix: hHmix,
                bmix: bmix,
                hnew: hnew,
                hHnew: hHnew,
                bnew: bnew,
                breset: breset,
                hHreset: hHreset,
                hreset: breset
            );
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(_inputDimension);
            writer.Write(_outputDimension);

            _hmix.Save(writer);
            _hHmix.Save(writer);
            _bmix.Save(writer);
            _hnew.Save(writer);
            _hHnew.Save(writer);
            _bnew.Save(writer);
            _hreset.Save(writer);
            _hHreset.Save(writer);
            _breset.Save(writer);
        }
    }
}
