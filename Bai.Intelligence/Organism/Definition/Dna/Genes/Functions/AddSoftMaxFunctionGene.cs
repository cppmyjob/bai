using Bai.Intelligence.Cpu;
using Bai.Intelligence.Organism.Functions;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes.Functions
{
    public class AddSoftMaxFunctionGene : BaseFunctionGene
    {
        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null)
                return;

            context.Neuron.Function = new SoftMaxFunction();
            context.Neuron.Outputs = OutputIndexes;

        }
    }
}
