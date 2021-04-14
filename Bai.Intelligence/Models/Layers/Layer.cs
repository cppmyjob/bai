using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Organism.Definition.Dna.Genes;

namespace Bai.Intelligence.Models.Layers
{
    public abstract class Layer
    {
        public abstract int GetInputCount();
        public abstract int GetOutputCount();

        public abstract List<BaseGene> Compile(SequentialContext context);
    }
}
