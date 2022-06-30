namespace YARC.DL
{
    public class LossMultiDimensionalBinary : ILoss
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

            for (int i = 0; i < targetOutput.W.Length; i++)
            {
                if (targetOutput.W[i] >= 0.5 && actualOutput.W[i] < 0.5)
                {
                    return 1;
                }
                if (targetOutput.W[i] < 0.5 && actualOutput.W[i] >= 0.5)
                {
                    return 1;
                }
            }
            return 0;
        }

    }
}
