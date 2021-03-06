﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna;
using Bai.Intelligence.Organism.Definition.Dna.Genes;
using Bai.Intelligence.Organism.Definition.Dna.Genes.Functions;
using Bai.Intelligence.Tests.Infrastructure.Stubs;

namespace Bai.Intelligence.Tests.Infrastructure
{
    public class TestEnvBase
    {
        public OptimizerStub Optimizer = new OptimizerStub();


        #region Simple Neuron

        public NetworkDefinition CreateSimpleNeuron()
        {
            var result = new NetworkDefinition
            {
                InputCount = 3, 
                OutputCount = 1,
                Chromosomes = new List<Chromosome> { CreateSimpleChromosome() }
            };
            return result;
        }

        private Chromosome CreateSimpleChromosome()
        {
            var result = new Chromosome {
                Dna1 = CreateSimpleDna(), 
                Dna2 = CreateSimpleDna()
            };
            return result;
        }

        private NeuronDna CreateSimpleDna()
        {
            var result = new NeuronDna
                         {
                             Genes = CreateSimpleNeuronGenes()
                         };
            return result;
        }

        private List<BaseGene> CreateSimpleNeuronGenes()
        {
            var genes = new List<BaseGene>
                        {
                            new CreateNeuronGene(),
                            new AddInputsGene
                            {
                                Inputs = new List<NeuronInput>
                                         {
                                             new NeuronInput {SourceIndex = 2, Weight = 1.1F},
                                             new NeuronInput {SourceIndex = 0, Weight = 2.2F},
                                             new NeuronInput {SourceIndex = 1, Weight = 3.3F},
                                         }
                            },
                            new AddSigmoidFunctionGene {
                                Alfa = 0.1F,
                                OutputIndexes = new []{ 3 }
                            }
                        };
            return genes;
        }

        #endregion

        #region 2LayersNetwork


        public NetworkDefinition Create2LayersNetwork()
        {
            var result = new NetworkDefinition
                         {
                             InputCount = 3,
                             OutputCount = 1,
                             Chromosomes = new List<Chromosome> { Create2LayersNetworkChromosome() }
                         };
            return result;
        }

        private Chromosome Create2LayersNetworkChromosome()
        {
            var result = new Chromosome
                         {
                             Dna1 = Create2LayersNetworkDna(),
                             Dna2 = Create2LayersNetworkDna()
                         };
            return result;
        }

        private NeuronDna Create2LayersNetworkDna()
        {
            var result = new NeuronDna
                         {
                             Genes = Create2LayersNetworkGenes()
                         };
            return result;
        }

        private List<BaseGene> Create2LayersNetworkGenes()
        {
            var genes = new List<BaseGene>
                        {
                            // --- Neuron 0.0
                            new CreateNeuronGene(),
                            new AddInputsGene
                            {
                                Inputs = new List<NeuronInput>
                                         {
                                             new NeuronInput {SourceIndex = 0, Weight = 0.1F},
                                             new NeuronInput {SourceIndex = 1, Weight = 0.2F},
                                             new NeuronInput {SourceIndex = 2, Weight = 0.3F},
                                         }
                            },
                            new AddSigmoidFunctionGene {
                                                           Alfa = 0.11F,
                                                           OutputIndexes = new []{4}
                                                       },
                            // --- Neuron 0.1
                            new CreateNeuronGene(),
                            new AddInputsGene
                            {
                                Inputs = new List<NeuronInput>
                                         {
                                             new NeuronInput {SourceIndex = 0, Weight = 0.4F},
                                             new NeuronInput {SourceIndex = 1, Weight = 0.5F},
                                             new NeuronInput {SourceIndex = 2, Weight = 0.6F},
                                         }
                            },
                            new AddSigmoidFunctionGene {
                                                           Alfa = 0.22F,
                                                           OutputIndexes = new []{5}
                                                       },

                            // --- Neuron 0.2
                            new CreateNeuronGene(),
                            new AddInputsGene
                            {
                                Inputs = new List<NeuronInput>
                                         {
                                             new NeuronInput {SourceIndex = 0, Weight = 0.7F},
                                             new NeuronInput {SourceIndex = 1, Weight = 0.8F},
                                             new NeuronInput {SourceIndex = 2, Weight = 0.9F},
                                         }
                            },
                            new AddSigmoidFunctionGene {
                                                           Alfa = 0.33F,
                                                           OutputIndexes = new []{6}
                                                       },


                            // --- Neuron 1.0
                            new CreateNeuronGene(),
                            new AddInputsGene
                            {
                                Inputs = new List<NeuronInput>
                                         {
                                             new NeuronInput {SourceIndex = 4, Weight = 1.1F},
                                             new NeuronInput {SourceIndex = 5, Weight = 2.2F},
                                             new NeuronInput {SourceIndex = 6, Weight = 3.3F},
                                         }
                            },
                            new AddSigmoidFunctionGene {
                                                           Alfa = 0.44F,
                                                           OutputIndexes = new []{3}
                                                       },


                        };
            return genes;
        }

        #endregion


        #region Neuron With Many To Many Function 1 layers

        public NetworkDefinition CreateNeuronWithManyToManyFunction()
        {
            var result = new NetworkDefinition
                         {
                             InputCount = 3,
                             OutputCount = 3,
                             Chromosomes = new List<Chromosome> { CreateWithManyToManyFunctionChromosome() }
                         };
            return result;
        }

        private Chromosome CreateWithManyToManyFunctionChromosome()
        {
            var result = new Chromosome
                         {
                             Dna1 = CreateWithManyToManyFunctionDna(),
                             Dna2 = CreateWithManyToManyFunctionDna()
                         };
            return result;
        }

        private NeuronDna CreateWithManyToManyFunctionDna()
        {
            var result = new NeuronDna
                         {
                             Genes = CreateWithManyToManyFunctionGenes()
                         };
            return result;
        }

        private List<BaseGene> CreateWithManyToManyFunctionGenes()
        {
            var genes = new List<BaseGene>
                        {
                            new CreateNeuronGene(),
                            new AddInputsGene
                            {
                                Inputs = new List<NeuronInput>
                                         {
                                             new NeuronInput {SourceIndex = 2, Weight = 1.1F},
                                             new NeuronInput {SourceIndex = 0, Weight = 2.2F},
                                             new NeuronInput {SourceIndex = 1, Weight = 3.3F},
                                         }
                            },
                            new AddSoftMaxFunctionGene {
                                                           OutputIndexes = new [] {3, 4, 5}
                                                       }
                        };
            return genes;
        }

        #endregion


    }
}
