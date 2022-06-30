namespace YARC.DL
{
    public class Lstm : ILayer
    {
        private readonly int _inputDimension;
        private readonly int _outputDimension;

        private readonly Matrix _wix;
        private readonly Matrix _wih;
        private readonly Matrix _inputBias;
        private readonly Matrix _wfx;
        private readonly Matrix _wfh;
        private readonly Matrix _forgetBias;
        private readonly Matrix _wox;
        private readonly Matrix _woh;
        private readonly Matrix _outputBias;
        private readonly Matrix _wcx;
        private readonly Matrix _wch;
        private readonly Matrix _cellWriteBias;

        private Matrix _hiddenContext;
        private Matrix _cellContext;

        private readonly INonlinearity _inputGateActivation = new SigmoidUnit();
        private readonly INonlinearity _forgetGateActivation = new SigmoidUnit();
        private readonly INonlinearity _outputGateActivation = new SigmoidUnit();
        private readonly INonlinearity _cellInputActivation = new TanhUnit();
        private readonly INonlinearity _cellOutputActivation = new TanhUnit();

        public Lstm(
            int inputDimension, 
            int outputDimension, 
            Matrix wix, 
            Matrix wih, 
            Matrix inputBias,
            Matrix wfx,
            Matrix wfh,
            Matrix forgetBias,
            Matrix wox,
            Matrix woh,
            Matrix outputBias,
            Matrix wcx,
            Matrix wch,
            Matrix cellWriteBias
            )
        {
            _inputDimension = inputDimension;
            _outputDimension = outputDimension;
            _wix = wix;
            _wih = wih;
            _inputBias = inputBias;
            _wfx = wfx;
            _wfh = wfh;
            _forgetBias = forgetBias;
            _wox = wox;
            _woh = woh;
            _outputBias = outputBias;
            _wcx = wcx;
            _wch = wch;
            _cellWriteBias = cellWriteBias;
        }

        public Lstm(int inputDimension, int outputDimension, double initParamsStdDev, Random rng)
        {
            this._inputDimension = inputDimension;
            this._outputDimension = outputDimension;

            _wix = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _wih = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            _inputBias = new Matrix(outputDimension);

            _wfx = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _wfh = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            //set forget bias to 1.0, as described here: http://jmlr.org/proceedings/papers/v37/jozefowicz15.pdf
            _forgetBias = Matrix.Ones(outputDimension, 1);

            _wox = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _woh = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            _outputBias = new Matrix(outputDimension);

            _wcx = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            _wch = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            _cellWriteBias = new Matrix(outputDimension);
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            //input gate
            Matrix sum0 = g.Mul(_wix, input);
            Matrix sum1 = g.Mul(_wih, _hiddenContext);
            Matrix inputGate = g.Nonlin(_inputGateActivation, g.Add(g.Add(sum0, sum1), _inputBias));

            //forget gate
            Matrix sum2 = g.Mul(_wfx, input);
            Matrix sum3 = g.Mul(_wfh, _hiddenContext);
            Matrix forgetGate = g.Nonlin(_forgetGateActivation, g.Add(g.Add(sum2, sum3), _forgetBias));

            //output gate
            Matrix sum4 = g.Mul(_wox, input);
            Matrix sum5 = g.Mul(_woh, _hiddenContext);
            Matrix outputGate = g.Nonlin(_outputGateActivation, g.Add(g.Add(sum4, sum5), _outputBias));

            //write operation on cells
            Matrix sum6 = g.Mul(_wcx, input);
            Matrix sum7 = g.Mul(_wch, _hiddenContext);
            Matrix cellInput = g.Nonlin(_cellInputActivation, g.Add(g.Add(sum6, sum7), _cellWriteBias));

            //compute new cell activation
            Matrix retainCell = g.Elmul(forgetGate, _cellContext);
            Matrix writeCell = g.Elmul(inputGate, cellInput);
            Matrix cellAct = g.Add(retainCell, writeCell);

            //compute hidden state as gated, saturated cell activations
            Matrix output = g.Elmul(outputGate, g.Nonlin(_cellOutputActivation, cellAct));

            //rollover activations for next iteration
            _hiddenContext = output;
            _cellContext = cellAct;

            return output;
        }

        public void ResetState()
        {
            _hiddenContext = new Matrix(_outputDimension);
            _cellContext = new Matrix(_outputDimension);
        }

        public void ResetState(Lstm lstm)
        {
            _hiddenContext = lstm._hiddenContext;
            _cellContext = lstm._cellContext;
        }

        public List<Matrix> GetParameters()
        {
            List<Matrix> result = new List<Matrix>();
            result.Add(_wix);
            result.Add(_wih);
            result.Add(_inputBias);
            result.Add(_wfx);
            result.Add(_wfh);
            result.Add(_forgetBias);
            result.Add(_wox);
            result.Add(_woh);
            result.Add(_outputBias);
            result.Add(_wcx);
            result.Add(_wch);
            result.Add(_cellWriteBias);
            return result;
        }

        public static Lstm Load(BinaryReader reader)
        {
            var inputDimension = reader.ReadInt32();
            var outputDimension = reader.ReadInt32();

            var wix = Matrix.Load(reader);
            var wih = Matrix.Load(reader);
            var inputBias = Matrix.Load(reader);

            var wfx = Matrix.Load(reader);
            var wfh = Matrix.Load(reader);
            var forgetBias = Matrix.Load(reader);

            var wox = Matrix.Load(reader);
            var woh = Matrix.Load(reader);
            var outputBias = Matrix.Load(reader);

            var wcx = Matrix.Load(reader);
            var wch = Matrix.Load(reader);
            var cellWriteBias = Matrix.Load(reader);

            return new Lstm(
                inputDimension: inputDimension,
                outputDimension: outputDimension,
                wix: wix,
                wih: wih,
                inputBias: inputBias,
                wfx: wfx,
                wfh: wfh,
                forgetBias: forgetBias,
                wox: wox,
                woh: woh,
                outputBias: outputBias,
                wcx: wcx,
                wch: wch,
                cellWriteBias: cellWriteBias);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(_inputDimension);
            writer.Write(_outputDimension);

            _wix.Save(writer);
            _wih.Save(writer);
            _inputBias.Save(writer);

            _wfx.Save(writer);
            _wfh.Save(writer);
            _forgetBias.Save(writer);

            _wox.Save(writer);
            _woh.Save(writer);
            _outputBias.Save(writer);

            _wcx.Save(writer);
            _wch.Save(writer);
            _cellWriteBias.Save(writer);
        }

        public Matrix Context()
        {
            return this._hiddenContext;
        }
    }
}
