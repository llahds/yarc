namespace YARC.DL
{
    public class CrossEntropy : ILoss
    {

        public void Backward(Matrix actualOutput, Matrix targetOutput)
        {
            for (int i = 0; i < targetOutput.W.Length; i++)
            {
                double errDelta = actualOutput.W[i] - targetOutput.W[i];
                actualOutput.Dw[i] += errDelta;
            }

        }

        public double Measure(Matrix target, Matrix actual)
        {
            var crossentropy = 0.0;

            for (int i = 0; i < actual.W.Length; i++)
            {

                crossentropy -= (target.W[i] * Math.Log(actual.W[i] + 1e-15)) +
                                ((1 - target.W[i]) * Math.Log((1 + 1e-15) - actual.W[i]));
            }


            return crossentropy;
        }




    }
}
