using Bai.Intelligence.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Definition.Dna;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Definition.Dna.Genes.Functions;

namespace Bai.Intelligence.Tests.Infrastructure
{
    public class TestEnvBase
    {
        public NetworkDefinition CreateSimpleNeuron()
        {
            var result = new NetworkDefinition
            {
                InputCount = 3, 
                OutputCount = 1,
                Chromosomes = new []{ CreateSimpleChromosome() }
            };
            return result;
        }

        private Chromosome CreateSimpleChromosome()
        {
            var result = new Chromosome {
                Man = CreateSimpleDna(), 
                Woman = CreateSimpleDna()
            };
            return result;
        }

        private NeuronDna CreateSimpleDna()
        {
            var result = new NeuronDna
                         {
                             Genes = CreateSimpleNeuronGenes().ToArray()
                         };
            return result;
        }

        private List<BaseGene> CreateSimpleNeuronGenes()
        {
            var genes = new List<BaseGene>
                        {
                            new CreateNeuronGene(),
                            new AddInputsGene()
                            {
                                Dominant = true,
                                Inputs = new[]
                                         {
                                             new NeuronInput() {SourceIndex = 2, Weight = 1.1F},
                                             new NeuronInput() {SourceIndex = 0, Weight = 2.2F},
                                             new NeuronInput() {SourceIndex = 1, Weight = 3.3F},
                                         }
                            },
                            new AddSigmoidFunctionGene {
                                Dominant = true, 
                                Alfa = 4.4F,
                                OutputIndex = 3
                            }
                        };
            return genes;
        }
    }
}
