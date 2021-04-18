using System;
using System.Collections.Generic;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Models.Initializers;
using Bai.Intelligence.Organism.Definition.Dna.Genes;
using Bai.Intelligence.Organism.Definition.Dna.Genes.Functions;

namespace Bai.Intelligence.Models.Layers
{
    public class Dense : Layer
    {
        private readonly ActivationType _activation;
        private readonly int _inputDim;
        private readonly int _units;

        public Dense(int units, ActivationType activation = ActivationType.Linear, int inputDim = 0)
        {
            _units = units;
            _activation = activation;
            _inputDim = inputDim;
        }

        public override int GetInputCount()
        {
            if (_inputDim == 0)
                // TODO
                throw new Exception();
            return _inputDim;
        }

        public override int GetOutputCount()
        {
            return _units;
        }

        public override List<BaseGene> Compile(SequentialContext context)
        {
            switch (_activation)
            {
                case ActivationType.Softmax:
                    return SoftMaxNeuronsGeneration(context);
                default:
                    return DefaultNeuronsGeneration(context, _activation);
            }
        }

        private List<BaseGene> SoftMaxNeuronsGeneration(SequentialContext context)
        {
            var result = new List<BaseGene>();

            context.InsertLayer();
            var linearGenes = DefaultNeuronsGeneration(context, ActivationType.Linear);
            result.AddRange(linearGenes);

            context.NextLayer(_units);

            var initializer = CreateInitializer();
            var initializerResult = initializer.GetValues(_units, _units);
            AddNeuron(context, result, 0, initializerResult, ActivationType.Softmax);

            return result;
        }

        private GlorotUniform CreateInitializer()
        {
            return new GlorotUniform();
        }

        private List<BaseGene> DefaultNeuronsGeneration(SequentialContext context, ActivationType activationType)
        {
            var initializer = CreateInitializer();
            var initializerResult = initializer.GetValues(context.PreviousInputCount, _units);

            var result = new List<BaseGene>();
            for (var i = 0; i < _units; i++)
            {
                AddNeuron(context, result, i, initializerResult, activationType);
            }

            return result;
        }

        private void AddNeuron(SequentialContext context, List<BaseGene> result, 
            int neuronIndex, InitializerResult initializerResult, ActivationType activationType)
        {
            var inputs = CreateInputs(context, neuronIndex, initializerResult);
            result.AddRange(new BaseGene[]
                            {
                                new CreateNeuronGene(),
                                new AddInputsGene
                                {
                                    InitMaxWeight = initializerResult.MaxWeight,
                                    InitMinWeight = initializerResult.MinWeight,
                                    Inputs = inputs
                                },
                                CreateFunctionGene(context, inputs, activationType)
                            });
        }

        private BaseFunctionGene CreateFunctionGene(SequentialContext context, List<NeuronInput> inputs, 
            ActivationType activationType)
        {
            switch (activationType)
            {
                case ActivationType.Linear:
                    return new AddLinearFunctionGene {
                        OutputIndexes = new[] { context.OutputOffset++ },
                    };
                // TODO Should O change Alfa = 1
                case ActivationType.Sigmoid:
                    return new AddSigmoidFunctionGene
                           {
                               OutputIndexes = new []{ context.OutputOffset++ },
                               Alfa = 1
                           };
                case ActivationType.Relu:
                    return new AddReluFunctionGene 
                           {
                                OutputIndexes = new[] { context.OutputOffset++ },
                           };
                case ActivationType.Softmax:
                    var outputIndexes = new int[inputs.Count];
                    for (var i = 0; i < outputIndexes.Length; i++)
                    {
                        outputIndexes[i] = context.OutputOffset++;
                    }
                    return new AddSoftMaxFunctionGene
                           {
                               OutputIndexes = outputIndexes,
                           };
                default:
                    throw new NotSupportedException();
            }
        }

        private List<NeuronInput> CreateInputs(SequentialContext context, int neuronIndex, InitializerResult initializerResult)
        {
            var result = new List<NeuronInput>();
            var inputOffset = context.InputOffset;
            for (var i = 0; i < context.PreviousInputCount; i++)
            {
                var input = new NeuronInput {
                    Weight = initializerResult.Weights[neuronIndex * context.PreviousInputCount + i],
                    SourceIndex = inputOffset++
                };
                result.Add(input);
            }
            return result;
        }
    }
}