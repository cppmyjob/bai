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

            ShouldCompileCorrectGenesCheckGenes(manGenes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes);
        }

        private static void ShouldCompileCorrectGenesCheckGenes(BaseGene[] manGenes)
        {
            var inputs = manGenes.Where(t => t is AddInputsGene)
                .Cast<AddInputsGene>().SelectMany(t => t.Inputs).ToArray();
            Assert.AreEqual(20, inputs.Length);

            var expectedInputIndexes = new int[] {0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 6, 7, 8, 6, 7, 8, 9, 10};
            Assert.AreEqual(expectedInputIndexes, inputs.Select(t => t.SourceIndex).ToArray());

            var outputs = manGenes.Where(t => t is BaseFunctionGene)
                .Cast<BaseFunctionGene>().SelectMany(t => t.OutputIndexes).ToArray();
            var expectedOutputIndexes = new int[] {5, 6, 7, 8, 9, 4};
            Assert.AreEqual(expectedOutputIndexes, outputs);
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

            ShouldCompileCorrectSoftMaxGenesCheckGenes(manGenes);
            ShouldCompileCorrectSoftMaxGenesCheckGenes(womanGenes);
        }

        private static void ShouldCompileCorrectSoftMaxGenesCheckGenes(BaseGene[] manGenes)
        {
            var inputs = manGenes.Where(t => t is AddInputsGene)
                .Cast<AddInputsGene>().SelectMany(t => t.Inputs).ToArray();

            var expectedInputIndexes = new int[] { 0, 1, 2, 0, 1, 2, 5, 6 };
            Assert.AreEqual(expectedInputIndexes, inputs.Select(t => t.SourceIndex).ToArray());

            var outputs = manGenes.Where(t => t is BaseFunctionGene)
                .Cast<BaseFunctionGene>().SelectMany(t => t.OutputIndexes).ToArray();
            var expectedOutputIndexes = new int[] { 5, 6, 3, 4 };
            Assert.AreEqual(expectedOutputIndexes, outputs);
        }
    }
}
