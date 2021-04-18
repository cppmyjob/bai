using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes
{
    public class CreateNeuronGene: BaseGene
    {
        public override void Build(BuilderContext context)
        {
            
        }

        public override BaseGene Clone()
        {
            return new CreateNeuronGene();
        }

        public override void RandomizeValues(IRandom random)
        {
            
        }

        public override void Mutate(IRandom random)
        {
            
        }
    }
}
