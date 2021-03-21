using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Definition.Dna.Genes.Functions;

namespace Bai.Intelligence.Models.Layers
{
    public class Dense : Layer
    {
        private readonly int _units;
        private readonly ActivationType _activation;
        private readonly int _inputDim;

        public Dense(int units, ActivationType activation = ActivationType.Linear, int inputDim = 0)
        {
            _units = units;
            _activation = activation;
            _inputDim = inputDim;
        }


        public override List<BaseGene> Compile(Layer previousLayer)
        {
            var result = new List<BaseGene>();
            if (previousLayer == null && _inputDim > 0)
                // TODO new exception type
                throw new Exception();

            int inputCount;
            if (previousLayer == null)
                inputCount = _inputDim;
            else
                inputCount = previousLayer.;

            for (int i = 0; i < _units; i++)
            {
                AddNeuron(result);
            }

            return result;
        }

        private void AddNeuron(List<BaseGene> result)
        {
            result.AddRange(new BaseGene[]
                            {

                                new CreateNeuronGene(),
                                new AddInputsGene
                                {
                                    Dominant = true,
                                    Inputs = new[]
                                             {
                                                 new NeuronInput {SourceIndex = 2, Weight = 1.1F},
                                                 new NeuronInput {SourceIndex = 0, Weight = 2.2F},
                                                 new NeuronInput {SourceIndex = 1, Weight = 3.3F},
                                             }
                                },
                                new AddSigmoidFunctionGene
                                {
                                    Dominant = true,
                                    Alfa = 0.1F,
                                    OutputIndex = 3
                                }
                            });
        }
    }
}
