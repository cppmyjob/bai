using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Definition;
using Bai.Intelligence.Definition.Dna;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Models.Layers;

namespace Bai.Intelligence.Models
{
    public class Sequential
    {
        public List<Layer> Layers { get; } = new List<Layer>();

        public NetworkDefinition NetworkDefinition { get; private set; }

        public void Compile()
        {
            if (Layers.Count == 0)
                // TODO
                throw new Exception();

            var firstInputCount = Layers[0].GetInputCount();
            var finalOutputCount = Layers[Layers.Count - 1].GetOutputCount();
            var context = new SequentialContext(firstInputCount, finalOutputCount, Layers.Count);

            var genes = new List<BaseGene>();

            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                if (i == 0)
                {
                    context.Init();
                }
                else
                {
                    context.NextLayer(Layers[i - 1].GetOutputCount());
                }
                genes.AddRange(layer.Compile(context));
            }

            NetworkDefinition = new NetworkDefinition()
            {
                InputCount = firstInputCount,
                OutputCount = finalOutputCount,
                Chromosomes = new Chromosome[] {
                                        new Chromosome
                                        {
                                            Man = new NeuronDna
                                                  {
                                                      Genes = genes.ToArray()
                                                  },
                                            Woman = new NeuronDna
                                                  {
                                                      Genes = genes.ToArray()
                                                  },
                                        }
                                    }
            };
        }

    }
}
