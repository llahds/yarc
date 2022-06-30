using YARC.DL;

namespace YARC.DL.Training.Backprop
{
    public class NonLinearityOperation : IBackpropOperation
    {
        private readonly INonlinearity neuron;
        private readonly Matrix m;
        private readonly Matrix returnObj;

        public NonLinearityOperation(INonlinearity neuron, Matrix m, Matrix returnObj)
        {
            this.neuron = neuron;
            this.m = m;
            this.returnObj = returnObj;
        }

        public void Execute()
        {
            int n = m.W.Length;

            for (int i = 0; i < n; i++)
            {
                m.Dw[i] += neuron.Backward(m.W[i]) * returnObj.Dw[i];
            }
        }
    }
}
