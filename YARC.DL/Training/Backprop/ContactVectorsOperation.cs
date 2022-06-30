using YARC.DL;

namespace YARC.DL.Training.Backprop
{
    public class ContactVectorsOperation : IBackpropOperation
    {
        //private readonly Matrix m1;
        //private readonly Matrix m2;
        private readonly Matrix[] matrices;
        private readonly Matrix returnObj;

        public ContactVectorsOperation(Matrix m1, Matrix m2, Matrix returnObj)
        {
            this.matrices = new[] { m1, m2 };
            this.returnObj = returnObj;
        }

        public ContactVectorsOperation(Matrix returnObj, params Matrix[] matrices)
        {
            this.returnObj = returnObj;
            this.matrices = matrices;
        }

        public void Execute()
        {
            var index = 0;
            foreach (var m in this.matrices)
            {
                for (int i = 0; i < m.W.Length; i++)
                {
                    m.W[i] = returnObj.W[index];
                    m.Dw[i] = returnObj.Dw[index];
                    m.StepCache[i] = returnObj.StepCache[index];
                    index++;
                }
            }

            //int index0 = 0;
            //for (int i = 0; i < m1.W.Length; i++)
            //{
            //    m1.W[i] = returnObj.W[index0];
            //    m1.Dw[i] = returnObj.Dw[index0];
            //    m1.StepCache[i] = returnObj.StepCache[index0];
            //    index0++;
            //}
            //for (int i = 0; i < m2.W.Length; i++)
            //{
            //    m2.W[i] = returnObj.W[index0];
            //    m2.Dw[i] = returnObj.Dw[index0];
            //    m2.StepCache[i] = returnObj.StepCache[index0];
            //    index0++;
            //}
        }
    }
}
