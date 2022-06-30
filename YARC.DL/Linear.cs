namespace YARC.DL
{
    public class Linear : ILayer
    {
        private readonly Matrix _w;

        public Linear(Matrix w)
        {
            _w = w;
        }

        public Linear(int inputDimension, int outputDimension, double initParamsStdDev, Random rng)
        {
            _w = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            Matrix returnObj = g.Mul(_w, input);
            return returnObj;
        }

        public void ResetState()
        {

        }

        public List<Matrix> GetParameters()
        {
            List<Matrix> result = new List<Matrix>();
            result.Add(_w);
            return result;
        }

        public void Save(BinaryWriter writer)
        {
            _w.Save(writer);
        }

        public static Linear Load(BinaryReader reader)
        {
            var w = Matrix.Load(reader);
            return new Linear(w);
        }
    }
}
