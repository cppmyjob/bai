using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna;
using Bai.Intelligence.Organism.Definition.Dna.Genes;
using Bai.Intelligence.Tests.Infrastructure;
using Bai.Intelligence.Tests.Infrastructure.Stubs;
using Bai.Intelligence.Utils.Random;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Genetic
{
    public class CrossoverTests
    {
        public enum GeneType
        {
            Dominant1,
            Dominant2,
            Recessive1,
            Recessive2
        }

        private class TestEnv : TestEnvBase
        {
            public NetworkDefinition CreateNetworkDefinition(GeneType manGene, GeneType womanGene)
            {
                var result = new NetworkDefinition
                             {
                                 InputCount = 3,
                                 OutputCount = 1,
                                 Chromosomes = new List<Chromosome> { CreateSimpleChromosome(manGene, womanGene) }
                             };
                return result;
            }

            private Chromosome CreateSimpleChromosome(GeneType manGene, GeneType womanGene)
            {
                var result = new Chromosome
                             {
                                 Dna1 = CreateDna(manGene),
                                 Dna2 = CreateDna(womanGene)
                             };
                return result;
            }

            private NeuronDna CreateDna(GeneType geneType)
            {
                var result = new NeuronDna
                             {
                                 Genes = new List<BaseGene>
                                         {
                                             CreateGene(geneType)
                                         }
                             };
                return result;
            }

            private BaseGene CreateGene(GeneType geneType)
            {
                switch (geneType)
                {
                    case GeneType.Dominant1:
                        return new Dominant1Gene();
                    case GeneType.Dominant2:
                        return new Dominant2Gene();
                    case GeneType.Recessive1:
                        return new Recessive1Gene();
                    case GeneType.Recessive2:
                        return new Recessive2Gene();
                    default:
                        throw new NotSupportedException();
                }
            }

        }

        private TestEnv _env;
        private IRandom _random;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnv();
            _random = RandomFactory.Instance.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _random?.Dispose();
        }

        [Test]
        public void ShouldHaveCorrectNumberTest()
        {
            // ARRANGE
            var man = _env.CreateSimpleNeuron();
            var woman = _env.CreateSimpleNeuron();

            // ACT
            var crossover = new Crossover(_random);
            var result = crossover.Execute(man, woman);

            // ARRANGE
            Assert.AreEqual(3, result.InputCount);
            Assert.AreEqual(1, result.OutputCount);
            Assert.AreEqual(1, result.Chromosomes.Count);
            Assert.AreEqual(3, result.Chromosomes[0].Dna1.Genes.Count);
            Assert.AreEqual(3, result.Chromosomes[0].Dna2.Genes.Count);
        }


        [Test]
        [Repeat(100)]
        public void MendelFirstLawTest()
        {
            // ARRANGE
            var man = _env.CreateNetworkDefinition(GeneType.Dominant1, GeneType.Dominant2);
            var woman = _env.CreateNetworkDefinition(GeneType.Recessive1, GeneType.Recessive2);

            // ACT
            var crossover = new Crossover(_random);
            var result = crossover.Execute(man, woman);

            // ARRANGE
            var gene1 = result.Chromosomes.SelectMany(t => t.Dna1.Genes).Single();
            var gene2 = result.Chromosomes.SelectMany(t => t.Dna2.Genes).Single();

            Assert.True(gene1 is Dominant1Gene || gene1 is Dominant2Gene);
            Assert.True(gene2 is Recessive1Gene || gene2 is Recessive2Gene);
        }

        [Test]
        public void MendelSecondLawTest()
        {
            // ARRANGE
            var man = _env.CreateNetworkDefinition(GeneType.Dominant1, GeneType.Recessive1);
            var woman = _env.CreateNetworkDefinition(GeneType.Dominant2, GeneType.Recessive2);

            // ACT
            var crossover = new Crossover(_random);
            int D1D2Counter = 0;
            int D1R2Counter = 0;
            int D2R1Counter = 0;
            int R1R2Counter = 0;

            for (int i = 0; i < 1000; i++)
            {
                var result = crossover.Execute(man, woman);
                var gene1 = result.Chromosomes.SelectMany(t => t.Dna1.Genes).Single();
                var gene2 = result.Chromosomes.SelectMany(t => t.Dna2.Genes).Single();

                if (gene1 is Dominant1Gene && gene2 is Dominant2Gene)
                    ++D1D2Counter;
                if (gene1 is Dominant1Gene && gene2 is Recessive2Gene)
                    ++D1R2Counter;
                if (gene1 is Recessive1Gene && gene2 is Dominant2Gene)
                    ++D2R1Counter;
                if (gene1 is Recessive1Gene && gene2 is Recessive2Gene)
                    ++R1R2Counter;
            }


            // ARRANGE
            Assert.True(220 <= D1D2Counter && D1D2Counter <= 280, $"D1D2Counter:{D1D2Counter}");
            Assert.True(220 <= D1R2Counter && D1R2Counter <= 280, $"D1R2Counter:{D1R2Counter}");
            Assert.True(220 <= D2R1Counter && D2R1Counter <= 280, $"D2R1Counter:{D2R1Counter}");
            Assert.True(220 <= R1R2Counter && R1R2Counter <= 280, $"R1R2Counter:{R1R2Counter}");
        }

    }
}
