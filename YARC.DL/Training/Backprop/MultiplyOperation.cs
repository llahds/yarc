using YARC.DL;

namespace YARC.DL.Training.Backprop
{
    public class MultiplyOperation : IBackpropOperation
    {
        private readonly Matrix m1;
        private readonly Matrix m2;
        private readonly Matrix returnObj;

        public MultiplyOperation(Matrix m1, Matrix m2, Matrix returnObj)
        {
            this.m1 = m1;
            this.m2 = m2;
            this.returnObj = returnObj;
        }

        public void Execute()
        {
            int m1Rows = m1.Rows;
            int m1Cols = m1.Cols;
            int m2Cols = m2.Cols;
            int outcols = m2Cols;

            for (int i = 0; i < m1.Rows; i++)
            {
                int outcol = outcols * i;
                for (int j = 0; j < m2.Cols; j++)
                {
                    double b = returnObj.Dw[outcol + j];
                    for (int k = 0; k < m1.Cols; k++)
                    {
                        m1.Dw[m1Cols * i + k] += m2.W[m2Cols * k + j] * b;
                        m2.Dw[m2Cols * k + j] += m1.W[m1Cols * i + k] * b;
                    }
                }
            }
        }
    }
}
