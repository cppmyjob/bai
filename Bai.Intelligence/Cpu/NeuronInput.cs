namespace Bai.Intelligence.Cpu
{
    public class NeuronInput
    {
        public float Weight { get; set; }
        public int SourceIndex { get; set; }

        public NeuronInput Clone()
        {
            return new NeuronInput {
                Weight = Weight,
                SourceIndex = SourceIndex
            };
        }
    }
}
