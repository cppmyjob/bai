using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Functions;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes.Functions
{
    public class AddReluFunctionGene : BaseFunctionGene
    {
        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null)
                return;

            var function = new ReluFunction();
            context.Neuron.Function = function;
            context.Neuron.Outputs = OutputIndexes;
        }

        public override BaseGene Clone()
        {
            var result = new AddReluFunctionGene();
            SetClonedValues(result);
            return result;
        }

        public override void RandomizeValues(IRandom random)
        {
            // TODO for Outputs
        }

    }
}
