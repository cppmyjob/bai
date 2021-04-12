using System.Linq;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Definition.Dna.Genes.Functions;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
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
            model.Compile();

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(2, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Man.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Woman.Genes).ToArray();

            var expectedInputIndexes = new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 6, 7, 8, 6, 7, 8, 9, 10 };
            var expectedOutputIndexes = new int[] { 6, 7, 8, 9, 10, 4, 5 };
            ShouldCompileCorrectGenesCheckGenes(manGenes, expectedInputIndexes, expectedOutputIndexes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes, expectedInputIndexes, expectedOutputIndexes);
        }

        [Test]
        public void ShouldCompileCorrectSigmoidLayerGenes()
        {
            // ARRANGE
            var model = new Sequential();
            model.Layers.Add(new Dense(3, inputDim: 4, activation: ActivationType.Sigmoid));

            // ACT
            model.Compile();

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(3, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Man.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Woman.Genes).ToArray();

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
            model.Compile();

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(2, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Man.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Woman.Genes).ToArray();

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
            model.Compile();

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(4, model.NetworkDefinition.InputCount);
            Assert.AreEqual(1, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Man.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Woman.Genes).ToArray();

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
            model.Compile();

            // ASSERT
            Assert.IsNotNull(model.NetworkDefinition);
            Assert.AreEqual(3, model.NetworkDefinition.InputCount);
            Assert.AreEqual(2, model.NetworkDefinition.OutputCount);

            var manGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Man.Genes).ToArray();
            var womanGenes = model.NetworkDefinition.Chromosomes.SelectMany(t => t.Woman.Genes).ToArray();

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
