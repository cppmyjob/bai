using System;
using System.Collections.Generic;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Definition.Dna.Genes.Functions;
using Bai.Intelligence.Initializers;

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
            var weights = initializer.GetValues(_units, _units);
            AddNeuron(context, result, 0, weights, ActivationType.Softmax);

            return result;
        }

        private GlorotUniform CreateInitializer()
        {
            return new GlorotUniform();
        }

        private List<BaseGene> DefaultNeuronsGeneration(SequentialContext context, ActivationType activationType)
        {
            var initializer = CreateInitializer();
            var weights = initializer.GetValues(context.PreviousInputCount, _units);

            var result = new List<BaseGene>();
            for (var i = 0; i < _units; i++)
            {
                AddNeuron(context, result, i, weights, activationType);
            }

            return result;
        }

        private void AddNeuron(SequentialContext context, List<BaseGene> result, 
            int neuronIndex, float[] weights, ActivationType activationType)
        {
            var inputs = CreateInputs(context, neuronIndex, weights);
            result.AddRange(new BaseGene[]
                            {
                                new CreateNeuronGene(),
                                new AddInputsGene
                                {
                                    Inputs = inputs
                                },
                                CreateFunctionGene(context, inputs, activationType)
                            });
        }

        private BaseFunctionGene CreateFunctionGene(SequentialContext context, NeuronInput[] inputs, 
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
                case ActivationType.Softmax:
                    var outputIndexes = new int[inputs.Length];
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

        private NeuronInput[] CreateInputs(SequentialContext context, int neuronIndex, float[] weights)
        {
            var result = new List<NeuronInput>();
            var inputOffset = context.InputOffset;
            for (var i = 0; i < context.PreviousInputCount; i++)
            {
                var input = new NeuronInput {
                    Weight = weights[neuronIndex * context.PreviousInputCount + i],
                    SourceIndex = inputOffset++
                };
                result.Add(input);
            }
            return result.ToArray();
        }
    }
}