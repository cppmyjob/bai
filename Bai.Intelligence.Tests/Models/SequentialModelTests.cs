using System.Linq;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Data;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna.Genes;
using Bai.Intelligence.Organism.Definition.Dna.Genes.Functions;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Models
{
    public class SequentialModelTests
    {
        private TestEnvBase _env;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        [Test]
        public void ShouldCompileCorrectGenes()
        {

            // ARRANGE
            var model = new Sequential();
            model.Layers.Add(new Dense(3, inputDim: 4, activation: ActivationType.Sigmoid));
            model.Layers.Add(new Dense(2, activation: ActivationType.Softmax));

            // ACT
            model.Compile(_env.Optimizer);

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(2, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna1.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna2.Genes).ToArray();

            var expectedInputIndexes = new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 6, 7, 8, 6, 7, 8, 9, 10 };
            var expectedOutputIndexes = new int[] { 6, 7, 8, 9, 10, 4, 5 };
            ShouldCompileCorrectGenesCheckGenes(manGenes, expectedInputIndexes, expectedOutputIndexes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes, expectedInputIndexes, expectedOutputIndexes);
        }

        [Test]
        public void ShouldComputeCpuCorrect()
        {

            // ARRANGE
            var model = new Sequential();
            model.Layers.Add(new Dense(3, inputDim: 4, activation: ActivationType.Relu));
            model.Layers.Add(new Dense(2, activation: ActivationType.Softmax));

            model.Compile(_env.Optimizer);
            SetWeight(model.NetworkDefinition, 0, 0, 0.1F);
            SetWeight(model.NetworkDefinition, 0, 1, 0.2F);
            SetWeight(model.NetworkDefinition, 0, 2, 0.3F);
            SetWeight(model.NetworkDefinition, 0, 3, 0.4F);

            SetWeight(model.NetworkDefinition, 1, 0, -0.1F);
            SetWeight(model.NetworkDefinition, 1, 1, -0.2F);
            SetWeight(model.NetworkDefinition, 1, 2, -0.3F);
            SetWeight(model.NetworkDefinition, 1, 3, -0.4F);

            SetWeight(model.NetworkDefinition, 2, 0, 0.5F);
            SetWeight(model.NetworkDefinition, 2, 1, 0.6F);
            SetWeight(model.NetworkDefinition, 2, 2, 0.7F);
            SetWeight(model.NetworkDefinition, 2, 3, 0.8F);


            SetWeight(model.NetworkDefinition, 3, 0, 3.3F);
            SetWeight(model.NetworkDefinition, 3, 1, 4.4F);
            SetWeight(model.NetworkDefinition, 3, 2, 5.5F);

            SetWeight(model.NetworkDefinition, 4, 0, 6.6F);
            SetWeight(model.NetworkDefinition, 4, 1, 7.7F);
            SetWeight(model.NetworkDefinition, 4, 2, 8.8F);


            SetWeight(model.NetworkDefinition, 5, 0, 0.02F);
            SetWeight(model.NetworkDefinition, 5, 1, 0.01F);

            // ACT
            var builder = new CpuBuilder();

            var runtime = builder.Build(model.NetworkDefinition);
            var memory = new[] { 1F, 2F, 3F, 4F };

            var result = runtime.Compute(memory, new[] {new InputData {Offset = 0, Length = 4}});

            // ASSERT
            Assert.AreEqual(0.5384240912F, result[0], 0.0000001);
            Assert.AreEqual(0.4615759088f, result[1], 0.0000001);
        }

        private void SetWeight(NetworkDefinition definition, int neuronId, int inputId, float weight)
        {
            var inputGene = (AddInputsGene) definition.Chromosomes[0].Dna1.Genes[3 * neuronId + 1];
            inputGene.Inputs[inputId].Weight = weight;
            inputGene = (AddInputsGene)definition.Chromosomes[0].Dna2.Genes[3 * neuronId + 1];
            inputGene.Inputs[inputId].Weight = weight;
        }

        [Test]
        public void ShouldCompileCorrectSigmoidLayerGenes()
        {
            // ARRANGE
            var model = new Sequential();
            model.Layers.Add(new Dense(3, inputDim: 4, activation: ActivationType.Sigmoid));

            // ACT
            model.Compile(_env.Optimizer);

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(3, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna1.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna2.Genes).ToArray();

            var expectedInputIndexes = new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };
            var expectedOutputIndexes = new int[] { 4, 5, 6 };
            ShouldCompileCorrectGenesCheckGenes(manGenes, expectedInputIndexes, expectedOutputIndexes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes, expectedInputIndexes, expectedOutputIndexes);
        }


        [Test]
        public void ShouldCompileCorrect2SigmoidLayersGenes()
        {
            // ARRANGE
            var model = new Sequential();
            model.Layers.Add(new Dense(3, inputDim: 4, activation: ActivationType.Sigmoid));
            model.Layers.Add(new Dense(2, activation: ActivationType.Sigmoid));

            // ACT
            model.Compile(_env.Optimizer);

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(2, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna1.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna2.Genes).ToArray();

            var expectedInputIndexes = new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 6, 7, 8, 6, 7, 8 };
            var expectedOutputIndexes = new int[] { 6, 7, 8, 4, 5 };
            ShouldCompileCorrectGenesCheckGenes(manGenes, expectedInputIndexes, expectedOutputIndexes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes, expectedInputIndexes, expectedOutputIndexes);
        }

        [Test]
        public void ShouldCompileCorrect3SigmoidLayersGenes()
        {
            // ARRANGE
            var model = new Sequential();
            model.Layers.Add(new Dense(3, inputDim: 4, activation: ActivationType.Sigmoid));
            model.Layers.Add(new Dense(2, activation: ActivationType.Sigmoid));
            model.Layers.Add(new Dense(1, activation: ActivationType.Sigmoid));

            // ACT
            model.Compile(_env.Optimizer);

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(1, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna1.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna2.Genes).ToArray();

            var expectedInputIndexes = new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 5, 6, 7, 5, 6, 7, 8, 9 };
            var expectedOutputIndexes = new int[] { 5, 6, 7, 8, 9, 4 };
            ShouldCompileCorrectGenesCheckGenes(manGenes, expectedInputIndexes, expectedOutputIndexes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes, expectedInputIndexes, expectedOutputIndexes);
        }


        [Test]
        public void ShouldCompileCorrectSoftMaxGenes()
        {
            // ARRANGE
            var model = new Sequential();
            model.Layers.Add(new Dense(2, inputDim: 3, activation: ActivationType.Softmax));

            // ACT
            model.Compile(_env.Optimizer);

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(3, model.NetworkDefinition.InputCount);
            Assert.AreEqual(2, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna1.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Dna2.Genes).ToArray();

            var expectedInputIndexes = new int[] { 0, 1, 2, 0, 1, 2, 5, 6 };
            var expectedOutputIndexes = new int[] { 5, 6, 3, 4 };
            ShouldCompileCorrectGenesCheckGenes(manGenes, expectedInputIndexes, expectedOutputIndexes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes, expectedInputIndexes, expectedOutputIndexes);
        }

        private static void ShouldCompileCorrectGenesCheckGenes(BaseGene[] manGenes,
            int[] expectedInputIndexes, int[] expectedOutputIndexes)
        {
            var inputs = manGenes.Where(t => t is AddInputsGene)
                .Cast<AddInputsGene>().SelectMany(t => t.Inputs).ToArray();
            Assert.AreEqual(expectedInputIndexes, inputs.Select(t => t.SourceIndex).ToArray());

            var outputs = manGenes.Where(t => t is BaseFunctionGene)
                .Cast<BaseFunctionGene>().SelectMany(t => t.OutputIndexes).ToArray();
            Assert.AreEqual(expectedOutputIndexes, outputs);
        }
    }
}
