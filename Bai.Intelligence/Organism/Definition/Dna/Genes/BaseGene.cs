using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes
{
    public abstract class BaseGene
    {
        public abstract void Build(BuilderContext context);
        public abstract BaseGene Clone();
        public abstract void RandomizeValues(IRandom random);
        public abstract void Mutate(IRandom random);
        public abstract void CopyTo(BaseGene value);
    }

}
