using System.Linq;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Definition.Dna.Genes.Functions;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Models
{
    public class ModelCompileTests
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

            ShouldCompileCorrectGenesCheckGenes(manGenes);
            ShouldCompileCorrectGenesCheckGenes(womanGenes);
        }

        private static void ShouldCompileCorrectGenesCheckGenes(BaseGene[] manGenes)
        {
            var inputs = manGenes.Where(t => t is AddInputsGene)
                .Cast<AddInputsGene>().SelectMany(t => t.Inputs).ToArray();
            Assert.AreEqual(20, inputs.Length);

            var expectedInputIndexes = new int[] {0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3, 5, 6, 7, 5, 6, 7, 8, 9};
            Assert.AreEqual(expectedInputIndexes, inputs.Select(t => t.SourceIndex).ToArray());

            var outputs = manGenes.Where(t => t is BaseFunctionGene)
                .Cast<BaseFunctionGene>().SelectMany(t => t.OutputIndexes).ToArray();
            var expectedOutputIndexes = new int[] {5, 6, 7, 8, 9, 4};
            Assert.AreEqual(expectedOutputIndexes, outputs);
        }
    }
}
