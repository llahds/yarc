namespace YARC.DL
{
    public interface ILoss
    {
        void Backward(Matrix actualOutput, Matrix targetOutput);
        double Measure(Matrix actualOutput, Matrix targetOutput);
    }
}
