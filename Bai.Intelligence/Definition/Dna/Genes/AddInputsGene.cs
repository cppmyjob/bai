using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Definition.Dna.Genes
{
    public class AddInputsGene : BaseGene
    {
        public NeuronInput[] Inputs { get; set; }

        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null)
                return;
            context.Neuron.Inputs.AddRange(Inputs);
        }
    }
}
