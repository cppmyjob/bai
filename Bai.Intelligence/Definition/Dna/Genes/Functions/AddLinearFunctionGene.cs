using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Function;

namespace Bai.Intelligence.Definition.Dna.Genes.Functions
{
    public class AddLinearFunctionGene : BaseFunctionGene
    {
        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null)
                return;

            context.Neuron.Function = new LinearFunction();
            context.Neuron.Outputs = OutputIndexes;
        }
    }
}
