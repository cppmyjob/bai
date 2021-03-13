using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Definition.Dna.Genes
{
    public abstract class BaseGene
    {
        public bool Dominant { get; set; }
        public int Damage { get; set; }
        public abstract void Build(BuilderContext context);
    }

}
