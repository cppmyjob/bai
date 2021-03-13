using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Function;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Definition.Dna.Genes.Functions
{
    public class AddSigmoidFunctionGene: BaseFunctionGene
    {
        public float Alfa { get; set; }
        public override void Build(BuilderContext context)
        {
            if (context.Neuron == null) 
                return;

            var function = new SigmoidFunction { Alfa = Alfa};
            context.Neuron.Function = function;
            context.Neuron.Source = new NeuronSource { Index = OutputIndex };
        }
    }
}
