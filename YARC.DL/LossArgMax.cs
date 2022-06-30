namespace YARC.DL
{
    public class LossArgMax : ILoss
    {

        public void Backward(Matrix actualOutput, Matrix targetOutput)
        {
            for (int i = 0; i < targetOutput.W.Length; i++)
            {
                double errDelta = actualOutput.W[i] - targetOutput.W[i];
                actualOutput.Dw[i] += errDelta;
            }

        }

        public double Measure(Matrix actualOutput, Matrix targetOutput)
        {
            if (actualOutput.W.Length != targetOutput.W.Length)
            {
                throw new Exception("mismatch");
            }
            double maxActual = Double.PositiveInfinity;
            double maxTarget = Double.NegativeInfinity;
            int indxMaxActual = -1;
            int indxMaxTarget = -1;
            for (int i = 0; i < actualOutput.W.Length; i++)
            {
                if (actualOutput.W[i] > maxActual)
                {
                    maxActual = actualOutput.W[i];
                    indxMaxActual = i;
                }
                if (targetOutput.W[i] > maxTarget)
                {
                    maxTarget = targetOutput.W[i];
                    indxMaxTarget = i;
                }
            }
            if (indxMaxActual == indxMaxTarget)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
