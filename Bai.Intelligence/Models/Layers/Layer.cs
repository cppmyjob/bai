using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Definition.Dna.Genes;

namespace Bai.Intelligence.Models.Layers
{
    public abstract class Layer
    {
        protected virtual int GetInputCount()
        {
            return 0;
        }

        public abstract List<BaseGene> Compile(Layer previousLayer);
    }
}
