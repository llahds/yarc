namespace YARC.DL
{
    public interface INonlinearity
    {
        double Forward(double x);
        double Backward(double x);
    }
}
