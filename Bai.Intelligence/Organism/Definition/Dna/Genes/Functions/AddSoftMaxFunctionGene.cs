﻿using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Functions;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes.Functions
{
    public class AddSoftMaxFunctionGene : BaseFunctionGene
    {
        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null)
                return;

            context.Neuron.Function = new SoftMaxFunction();
            context.Neuron.Outputs = OutputIndexes;

        }

        public override BaseGene Clone()
        {
            var result = new AddSoftMaxFunctionGene();
            SetClonedValues(result);
            return result;
        }
        public override void RandomizeValues(IRandom random)
        {
            // TODO for Outputs
        }

    }
}
