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

        public void CopyTo(NeuronInput value)
        {
            value.Weight = Weight;
            value.SourceIndex = SourceIndex;
        }
    }
}
