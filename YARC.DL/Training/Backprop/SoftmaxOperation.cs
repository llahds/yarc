using YARC.DL;

namespace YARC.DL.Training.Backprop
{
    public class SoftmaxOperation : IBackpropOperation
    {
        private readonly Matrix matrix;
        private readonly Matrix result;

        public SoftmaxOperation(
            Matrix matrix,
            Matrix result
            )
        {
            this.matrix = matrix;
            this.result = result;
        }

        public void Execute()
        {
            var ss = 0.0;

            for (int ix = 0; ix < matrix.W.Length; ix++)
            {
                matrix.Dw[ix] = result.Dw[ix] * result.W[ix];
                ss += result.Dw[ix] * result.W[ix];
            }

            for (int ix = 0; ix < matrix.W.Length; ix++)
            {
                matrix.Dw[ix] -= ss * result.W[ix];
            }
        }
    }
}
