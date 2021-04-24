using System;
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

        public override void CopyTo(BaseGene value)
        {
            var blankValue = (BaseFunctionGene) value;
            if (OutputIndexes.Length != blankValue.OutputIndexes.Length)
                throw new Exception("BaseFunctionGene::CopyTo different OutputIndexes count");

            Array.Copy(OutputIndexes, blankValue.OutputIndexes, OutputIndexes.Length);
        }
    }
}
