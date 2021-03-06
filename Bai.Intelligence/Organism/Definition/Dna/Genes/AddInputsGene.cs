﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Organism.Definition.Dna.Genes
{
    public class AddInputsGene : BaseGene
    {
        public float? InitMaxWeight { get; set; }
        public float? InitMinWeight { get; set; }
        public List<NeuronInput> Inputs { get; set; }

        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null)
                return;
            context.Neuron.Inputs.AddRange(Inputs);
        }

        public override BaseGene Clone()
        {
            var result = new AddInputsGene();
            result.InitMaxWeight = InitMaxWeight;
            result.InitMinWeight = InitMinWeight;
            // TODO improve
            result.Inputs = Inputs.Select(t => t.Clone()).ToList();
            return result;
        }

        public override void RandomizeValues(IRandom random)
        {
            var limit2 = (InitMaxWeight - InitMinWeight).Value;
            var limit = limit2 / 2;
            foreach (var input in Inputs)
            {
                input.Weight =  (float)(random.NextDouble() * limit2 - limit);
            }
        }

        public override void Mutate(IRandom random)
        {
            var limit2 = (InitMaxWeight - InitMinWeight).Value;
            var limit = limit2 / 2;

            var count = (int) (Inputs.Count * random.NextDouble());
            for (int i = 0; i < count; i++)
            {
                var index = random.Next(Inputs.Count);
                Inputs[index].Weight = (float)(random.NextDouble() * limit2 - limit);
            }
        }

        public override void CopyTo(BaseGene value)
        {
            var blankValue = (AddInputsGene) value;
            blankValue.InitMaxWeight = InitMaxWeight;
            blankValue.InitMinWeight = InitMinWeight;

            if (Inputs.Count != blankValue.Inputs.Count)
                throw new Exception("AddInputsGene::CopyTo different Inputs count");

            for (int i = 0; i < Inputs.Count; i++)
            {
                Inputs[i].CopyTo(blankValue.Inputs[i]);
            }
        }
    }
}
