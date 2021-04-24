using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;
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

        public override BaseGene Clone()
        {
            var result = new AddSigmoidFunctionGene();
            result.Alfa = Alfa;
            SetClonedValues(result);
            return result;
        }

        public override void RandomizeValues(IRandom random)
        {
            // TODO for Outputs
        }

        public override void Mutate(IRandom random)
        {
            // TODO for Outputs
        }

        public override void CopyTo(BaseGene value)
        {
            base.CopyTo(value);
            ((AddSigmoidFunctionGene) value).Alfa = Alfa;
        }
    }
}
