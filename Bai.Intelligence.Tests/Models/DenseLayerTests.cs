using System.Collections.Generic;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Definition.Dna.Genes.Functions;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Models
{
    public class DenseLayerTests
    {
        private TestEnvBase _env;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        // TODO Check Weights test

        [Test]
        public void ShouldCreateCorrectGenes()
        {
            // ARRANGE
            var inputCount = 3;
            var outputCount = 2;

            var dense = new Dense(outputCount, activation: ActivationType.Sigmoid, inputDim: inputCount);

            // ACT
            var context = new SequentialContext();
            context.InputOffset = 0;
            context.PreviousInputCount = inputCount;
            context.OutputOffset = inputCount + outputCount;

            var genes = dense.Compile(context);

            // ASSERT
            Assert.AreEqual(6, genes.Count);

            ShouldCreateCorrectGenesAssertGene(genes, 0, 5);
            ShouldCreateCorrectGenesAssertGene(genes, 3, 6);
        }

        private void ShouldCreateCorrectGenesAssertGene(List<BaseGene> genes, int offset, 
            int outputValue)
        {
            var gene00 = genes[offset + 0];
            Assert.IsInstanceOf<CreateNeuronGene>(gene00);

            var gene01 = genes[offset + 1];
            Assert.IsInstanceOf<AddInputsGene>(gene01);
            var inputGene = (AddInputsGene)gene01;
            Assert.AreEqual(3, inputGene.Inputs.Length);
            Assert.AreEqual(0, inputGene.Inputs[0].SourceIndex);
            Assert.AreEqual(1, inputGene.Inputs[1].SourceIndex);
            Assert.AreEqual(2, inputGene.Inputs[2].SourceIndex);

            var gene02 = genes[offset + 2];
            Assert.IsInstanceOf<AddSigmoidFunctionGene>(gene02);
            var functionGene = (AddSigmoidFunctionGene)gene02;
            Assert.AreEqual(outputValue, functionGene.OutputIndexes[0]);
            Assert.AreEqual(1, functionGene.Alfa);
        }

    }
}
 