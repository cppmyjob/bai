using Bai.Intelligence.Function;

namespace Bai.Intelligence.Cpu
{

    public class Neuron
    {
        public NeuronInput[] Inputs { get; set; }
        public INeuronFunction Function { get; set; }
        public NeuronSource Source { get; set; }
    }
}
