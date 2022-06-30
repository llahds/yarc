namespace YARC.DL
{
    public class LossSoftmax : ILoss
    {
        public void Backward(Matrix logprobs, Matrix targetOutput)
        {
            throw new NotImplementedException();
        }

        public double Measure(Matrix logprobs, Matrix targetOutput)
        {
            throw new NotImplementedException();
        }

        public static Matrix GetSoftmaxProbs(Matrix logprobs, double temperature)
        {
            Matrix probs = new Matrix(logprobs.W.Length);
            if (temperature != 1.0)
            {
                for (int i = 0; i < logprobs.W.Length; i++)
                {
                    logprobs.W[i] /= temperature;
                }
            }
            double maxval = Double.NegativeInfinity;
            for (int i = 0; i < logprobs.W.Length; i++)
            {
                if (logprobs.W[i] > maxval)
                {
                    maxval = logprobs.W[i];
                }
            }
            double sum = 0;
            for (int i = 0; i < logprobs.W.Length; i++)
            {
                probs.W[i] = Math.Exp(logprobs.W[i] - maxval); //all inputs to exp() are non-positive
                sum += probs.W[i];
            }
            for (int i = 0; i < probs.W.Length; i++)
            {
                probs.W[i] /= sum;
            }
            return probs;
        }

        private static int GetTargetIndex(Matrix targetOutput)
        {
            for (int i = 0; i < targetOutput.W.Length; i++)
            {
                if (targetOutput.W[i] == 1.0)
                {
                    return i;
                }
            }
            throw new Exception("no target index selected");
        }
    }
}
