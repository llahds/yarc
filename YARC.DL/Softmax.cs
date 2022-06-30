namespace YARC.DL
{
    public class Softmax : ILayer
    {
        private Matrix _w;

        public Matrix Activate(Matrix input, Graph g)
        {
            _w = g.Softmax(input);

            return _w;
        }

        public List<Matrix> GetParameters()
        {
            return new List<Matrix> { _w };
        }

        public void ResetState()
        {
            
        }
    }
}
