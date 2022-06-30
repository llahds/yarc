namespace YARC.DL
{
    public class AverageLayer : ILayer
    {
        private readonly int rows;
        private readonly int cols;
        private Matrix _w;

        public AverageLayer(
            int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            if (this._w == null)
            {
                this._w = input.Clone();
            }
            else
            {
                this._w = g.Average(this._w, input);
            }

            return this._w;
        }

        public List<Matrix> GetParameters()
        {
            return new List<Matrix>
            {
                this._w
            };
        }

        public void ResetState()
        {
            this._w = null;
        }
    }
}
