namespace YARC.DL
{
    public class BidirectionalLstm : ILayer
    {
        private readonly Lstm forward;
        private readonly Lstm backward;
        private readonly ConcatLayer merge;
        private readonly FeedForward linear;

        public BidirectionalLstm(
            int inputCount,
            int hiddenCount,
            int outputCount,
            Random random)
        {
            this.forward = new Lstm(inputCount, hiddenCount, .1, random);
            this.backward = new Lstm(inputCount, hiddenCount, .1, random);
            this.merge = new ConcatLayer();
            this.linear = new FeedForward(hiddenCount * 2, outputCount, new RectifiedLinearUnit(), .1, random);
        }

        public BidirectionalLstm(
            Lstm forward,
            Lstm backward,
            ConcatLayer merge,
            FeedForward linear
            )
        {
            this.forward = forward;
            this.backward = backward;
            this.merge = merge;
            this.linear = linear;
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            throw new NotImplementedException();
        }

        public Matrix[] Activate(Matrix[] sequence, Graph g)
        {
            var forwardOutput = new List<Matrix>();
            foreach (var matrix in sequence)
            {
                forwardOutput.Add(this.forward.Activate(matrix, g));
            }

            var backwardOutput = new List<Matrix>();
            foreach (var matrix in sequence.Reverse())
            {
                backwardOutput.Add(this.backward.Activate(matrix, g));
            }

            var output = new List<Matrix>();
            backwardOutput.Reverse();
            for(var i = 0; i < sequence.Length; i++)
            {
                output.Add(this.linear.Activate(this.merge.Activate(g, forwardOutput[i], backwardOutput[i]), g));
            }
            return output.ToArray();
        }

        public List<Matrix> GetParameters()
        {
            var parameters = new List<Matrix>();

            parameters.AddRange(this.forward.GetParameters());
            parameters.AddRange(this.backward.GetParameters());
            parameters.AddRange(this.merge.GetParameters());
            parameters.AddRange(this.linear.GetParameters());

            return parameters;
        }

        public void ResetState()
        {
            this.forward.ResetState();
            this.backward.ResetState();
            this.merge.ResetState();
            this.linear.ResetState();
        }
    }
}
