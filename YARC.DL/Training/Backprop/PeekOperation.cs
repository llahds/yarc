using YARC.DL;

namespace YARC.DL.Training.Backprop
{
    public class PeekOperation : IBackpropOperation
    {
        private readonly Matrix m;
        private readonly int ix;
        private readonly Matrix returnObj;

        public PeekOperation(Matrix m, int ix, Matrix returnObj)
        {
            this.m = m;
            this.ix = ix;
            this.returnObj = returnObj;
        }

        public void Execute()
        {
            var d = m.Cols;
            for (int i = 0; i < d; i++)
            {
                m.Dw[d * ix + i] += returnObj.Dw[i];
            }
        }
    }
}
