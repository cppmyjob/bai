using Bai.Intelligence.Cpu;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes
{
    public class AddInputsGene : BaseGene
    {
        public float? InitMaxWeight { get; set; }
        public float? InitMinWeight { get; set; }
        public NeuronInput[] Inputs { get; set; }

        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null)
                return;
            context.Neuron.Inputs.AddRange(Inputs);
        }
    }
}
