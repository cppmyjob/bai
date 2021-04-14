using Bai.Intelligence.Cpu;
using Bai.Intelligence.Organism.Functions;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes.Functions
{
    public class AddSigmoidFunctionGene: BaseFunctionGene
    {
        public float Alfa { get; set; }
        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null) 
                return;

            var function = new SigmoidFunction { Alfa = Alfa};
            context.Neuron.Function = function;
            context.Neuron.Outputs = OutputIndexes;
        }
    }
}
