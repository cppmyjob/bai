using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Data;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Models.Optimizers;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna;
using Bai.Intelligence.Organism.Definition.Dna.Genes;

namespace Bai.Intelligence.Models
{
    public class Sequential
    {
        public List<Layer> Layers { get; } = new List<Layer>();

        public NetworkDefinition NetworkDefinition { get; private set; }

        private Optimizer _optimizer;

        public void Fit(DataArray x, DataArray y)
        {
            if (_optimizer == null)
                // TODO add correct exception
                throw new Exception("Model is not compiled");

            _optimizer.Run(NetworkDefinition);
        }

        public void Compile(Optimizer optimizer)
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
                                            Dna1 = new NeuronDna
                                                  {
                                                      Genes = genes.ToArray()
                                                  },
                                            Dna2 = new NeuronDna
                                                  {
                                                      Genes = genes.ToArray()
                                                  },
                                        }
                                    }
            };
        }

    }
}
