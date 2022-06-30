namespace YARC.DL
{
    public class Graph
    {
        public Graph()
        {

        }

        public virtual Matrix Peek(Matrix m, int ix)
        {
            var d = m.Cols;

            var weights = new double[m.Cols];

            for (int i = 0, n = d; i < n; i++)
            {
                weights[i] = m.W[d * ix + i];
            }

            return new Matrix(weights);
        }

        public virtual Matrix ConcatVectors(params Matrix[] matrices)
        {
            Matrix returnObj = new Matrix(matrices.Sum(M => M.Rows));

            int loc = 0;
            foreach (var matrix in matrices)
            {
                for (int i = 0; i < matrix.W.Length; i++)
                {
                    returnObj.W[loc] = matrix.W[i];
                    returnObj.Dw[loc] = matrix.Dw[i];
                    returnObj.StepCache[loc] = matrix.StepCache[i];
                    loc++;
                }
            }

            return returnObj;
        }

        public virtual Matrix Nonlin(INonlinearity neuron, Matrix m)
        {
            Matrix returnObj = new Matrix(m.Rows, m.Cols);
            int n = m.W.Length;
            for (int i = 0; i < n; i++)
            {
                returnObj.W[i] = neuron.Forward(m.W[i]);
            }

            return returnObj;
        }

        public virtual Matrix Mul(Matrix m1, Matrix m2)
        {
            if (m1.Cols != m2.Rows)
            {
                throw new Exception("matrix dimension mismatch");
            }

            int m1Rows = m1.Rows;
            int m1Cols = m1.Cols;
            int m2Cols = m2.Cols;
            Matrix returnObj = new Matrix(m1Rows, m2Cols);
            int outcols = m2Cols;
            for (int i = 0; i < m1Rows; i++)
            {
                int m1Col = m1Cols * i;
                for (int j = 0; j < m2Cols; j++)
                {
                    double dot = 0;
                    for (int k = 0; k < m1Cols; k++)
                    {
                        dot += m1.W[m1Col + k] * m2.W[m2Cols * k + j];
                    }
                    returnObj.W[outcols * i + j] = dot;
                }
            }

            return returnObj;
        }

        public virtual Matrix Add(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
            {
                throw new Exception("matrix dimension mismatch");
            }
            Matrix returnObj = new Matrix(m1.Rows, m1.Cols);
            for (int i = 0; i < m1.W.Length; i++)
            {
                returnObj.W[i] = m1.W[i] + m2.W[i];
            }
            
            return returnObj;
        }

        public Matrix OneMinus(Matrix m)
        {
            Matrix ones = Matrix.Ones(m.Rows, m.Cols);
            return Subtract(ones, m);
        }

        public Matrix Subtract(Matrix m1, Matrix m2)
        {
            return Add(m1, Neg(m2));
        }

        public Matrix Neg(Matrix m)
        {
            Matrix negones = Matrix.NegativeOnes(m.Rows, m.Cols);
            Matrix returnObj = Elmul(negones, m);
            return returnObj;
        }

        public virtual Matrix Elmul(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
            {
                throw new Exception("matrix dimension mismatch");
            }
            Matrix returnObj = new Matrix(m1.Rows, m1.Cols);
            for (int i = 0; i < m1.W.Length; i++)
            {
                returnObj.W[i] = m1.W[i] * m2.W[i];
            }

            return returnObj;
        }

        public virtual Matrix Softmax(Matrix m)
        {
            var res = new Matrix(m.Rows, m.Cols); // probability volume

            var maxval = -999999.0;
            for (int i = 0, n = m.W.Length; i < n; i++)
            {
                if (m.W[i] > maxval)
                {
                    maxval = m.W[i];
                }
            }

            var s = 0.0;
            for (int i = 0, n = m.W.Length; i < n; i++)
            {
                res.W[i] = Math.Exp(m.W[i] - maxval);
                s += res.W[i];
            }

            for (int i = 0, n = m.W.Length; i < n; i++)
            {
                res.W[i] /= s;
            }

            return res;
        }

        public virtual Matrix Average(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
            {
                throw new Exception("matrix dimension mismatch");
            }
            Matrix returnObj = new Matrix(m1.Rows, m1.Cols);
            for (int i = 0; i < m1.W.Length; i++)
            {
                returnObj.W[i] = (m1.W[i] + m2.W[i]) / 2.0;
            }

            return returnObj;
        }
    }
}
