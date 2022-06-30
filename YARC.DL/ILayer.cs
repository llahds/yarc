namespace YARC.DL
{
    public interface ILayer 
    {
        Matrix Activate(Matrix input, Graph g);
        void ResetState();
        List<Matrix> GetParameters();
    }
}
