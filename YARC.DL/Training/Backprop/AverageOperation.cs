using YARC.DL;
using System;

namespace YARC.DL.Training.Backprop
{
    public class AverageOperation : IBackpropOperation
    {
        private readonly Matrix m1;
        private readonly Matrix m2;
        private readonly Matrix returnObj;

        public AverageOperation(Matrix m1, Matrix m2, Matrix returnObj)
        {
            this.m1 = m1;
            this.m2 = m2;
            this.returnObj = returnObj;
        }

        public void Execute()
        {
            for (int i = 0; i < m1.W.Length; i++)
            {
                m1.Dw[i] += m2.W[i] * returnObj.Dw[i];
                m2.Dw[i] += m1.W[i] * returnObj.Dw[i];
            }
        }
    }
}
