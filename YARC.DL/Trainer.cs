namespace YARC.DL
{
    public class Trainer
    {
        public static double DecayRate = 0.999;
        public static double SmoothEpsilon = 1e-8;
        public static double GradientClipValue = 5;
        public static double Regularization = 0.000001; 

        public static void UpdateModelParams(double stepSize, params ILayer[] layers)
        {
            var parameters = new List<Matrix>();
            foreach (var layer in layers)
            {
                parameters.AddRange(layer.GetParameters());
            }
            foreach (var m in parameters)
            {
                for (int i = 0; i < m.W.Length; i++)
                {

                    // rmsprop adaptive learning rate
                    double mdwi = m.Dw[i];
                    m.StepCache[i] = m.StepCache[i] * DecayRate + (1 - DecayRate) * mdwi * mdwi;

                    // gradient clip
                    if (mdwi > GradientClipValue)
                    {
                        mdwi = GradientClipValue;
                    }
                    if (mdwi < -GradientClipValue)
                    {
                        mdwi = -GradientClipValue;
                    }

                    // update (and regularize)
                    m.W[i] += -stepSize * mdwi / Math.Sqrt(m.StepCache[i] + SmoothEpsilon) - Regularization * m.W[i];
                    m.Dw[i] = 0;
                }
            }
        }
    }
}
