using System.Collections.Generic;

namespace YARC.DL
{
    public class ConcatLayer : ILayer
    {
        private Matrix _w;

        public Matrix Activate(Matrix left, Matrix right, Graph g)
        {
            _w = g.ConcatVectors(left, right);
            return _w;
        }

        public Matrix Activate(Graph g, params Matrix[] matrices)
        {
            _w = g.ConcatVectors(matrices);
            return _w;
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            return input;
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
    }
}
