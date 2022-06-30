using YARC.DL;
using YARC.DL.Training.Backprop;
using System.Collections.Generic;

namespace YARC.DL.Training
{
    public class TrainingGraph : Graph
    {
        private readonly List<IBackpropOperation> _operations;

        public TrainingGraph()
        {
            _operations = new List<IBackpropOperation>();
        }

        public void Backward()
        {
            for (int i = _operations.Count - 1; i >= 0; i--)
            {
                _operations[i].Execute();
            }
        }

        public override Matrix Add(Matrix m1, Matrix m2)
        {
            var returnObj = base.Add(m1, m2);

            _operations.Add(new AddOperation(m1, m2, returnObj));

            return returnObj;
        }

        //public override Matrix ConcatVectors(Matrix m1, Matrix m2)
        //{
        //    var returnObj = base.ConcatVectors(m1, m2);

        //    _operations.Add(new ContactVectorsOperation(m1, m2, returnObj));

        //    return returnObj;
        //}

        public override Matrix ConcatVectors(params Matrix[] matrices)
        {
            var returnObj = base.ConcatVectors(matrices);

            //_operations.Add(new ContactVectorsOperation(m1, m2, returnObj));
            _operations.Add(new ContactVectorsOperation(returnObj, matrices));

            return returnObj;
        }

        public override Matrix Elmul(Matrix m1, Matrix m2)
        {
            var returnObj = base.Elmul(m1, m2);

            _operations.Add(new ElementwiseMultiplyOperation(m1, m2, returnObj));

            return returnObj;
        }

        public override Matrix Mul(Matrix m1, Matrix m2)
        {
            var returnObj = base.Mul(m1, m2);

            _operations.Add(new MultiplyOperation(m1, m2, returnObj));

            return returnObj;
        }

        public override Matrix Nonlin(INonlinearity neuron, Matrix m)
        {
            var returnObj = base.Nonlin(neuron, m);

            _operations.Add(new NonLinearityOperation(neuron, m, returnObj));

            return returnObj;
        }

        public override Matrix Peek(Matrix m, int ix)
        {
            var returnObj = base.Peek(m, ix);

            _operations.Add(new PeekOperation(m, ix, returnObj));

            return returnObj;
        }

        public override Matrix Softmax(Matrix m)
        {
            var result = base.Softmax(m);

            _operations.Add(new SoftmaxOperation(m, result));

            return result;
        }

        public override Matrix Average(Matrix m1, Matrix m2)
        {
            var result = base.Average(m1, m2);

            _operations.Add(new AverageOperation(m1, m2, result));

            return result;
        }
    }
}
