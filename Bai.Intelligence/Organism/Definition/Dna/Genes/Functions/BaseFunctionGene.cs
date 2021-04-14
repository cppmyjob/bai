using System.Linq;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes.Functions
{
    public abstract class BaseFunctionGene : BaseGene
    {
        public int[] OutputIndexes { get; set; }

        public void SetClonedValues(BaseFunctionGene clone)
        {
            // TODO improve
            clone.OutputIndexes = OutputIndexes.Select(t => t).ToArray();
        }
    }
}
