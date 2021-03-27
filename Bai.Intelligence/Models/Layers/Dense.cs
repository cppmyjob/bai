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
            var initializer = new GlorotUniform();
            var weights = initializer.GetValues(context.PreviousInputCount, _units);

            var result = new List<BaseGene>();
            for (var i = 0; i < _units; i++)
            {
                AddNeuron(context, result, i, weights);
            }
            return result;
        }

        private void AddNeuron(SequentialContext context, List<BaseGene> result, 
            int neuronIndex, float[] weights)
        {
            result.AddRange(new BaseGene[]
                            {
                                new CreateNeuronGene(),
                                new AddInputsGene
                                {
                                    Inputs = CreateInputs(context, neuronIndex, weights)
                                },
                                CreateFunctionGene(context)
                            });
        }

        private AddSigmoidFunctionGene CreateFunctionGene(SequentialContext context)
        {
            switch (_activation)
            {
                case ActivationType.Sigmoid:
                    return new AddSigmoidFunctionGene
                           {
                               OutputIndex = context.OutputOffset++,
                               Alfa = 1
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