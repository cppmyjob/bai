using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bai.Intelligence.Data;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Models.Optimizers;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna;
using Bai.Intelligence.Organism.Definition.Dna.Genes;
using Bai.Intelligence.Utils;

namespace Bai.Intelligence.Models
{
    public class Sequential
    {
        private readonly ILogger _logger;

        public Sequential(ILogger logger = null)
        {
            if (logger != null)
            {
                _logger = logger;
            }
            else
            {
                _logger = new ConsoleLogger();
            }
            
        }

        public List<Layer> Layers { get; } = new List<Layer>();

        public NetworkDefinition NetworkDefinition { get; private set; }

        private Optimizer _optimizer;

        public void Fit(DataArray x, DataArray y)
        {
            if (_optimizer == null)
                // TODO add correct exception
                throw new Exception("Model is not compiled");

            _optimizer.Run(_logger, NetworkDefinition, x, y);
        }

        public void Compile(Optimizer optimizer)
        {
            if (Layers.Count == 0)
                // TODO
                throw new Exception();
            _optimizer = optimizer;

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
                Chromosomes = new List<Chromosome> {
                                        new Chromosome
                                        {
                                            Dna1 = new NeuronDna
                                                  {
                                                      Genes = genes.ToList()
                                                  },
                                            Dna2 = new NeuronDna
                                                  {
                                                      Genes = genes.ToList()
                                                  },
                                        }
                                    }
            };
        }

    }
}
