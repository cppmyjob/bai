using System.Collections.Generic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna;
using Bai.Intelligence.Organism.Definition.Dna.Genes;

namespace Bai.Intelligence.Organism
{
    public class Crossover
    {
        private readonly IRandom _random;

        public Crossover(IRandom random)
        {
            _random = random;
        }

        public NetworkDefinition Execute(NetworkDefinition parent1, NetworkDefinition parent2)
        {
            var result = new NetworkDefinition();
            result.InputCount = parent1.InputCount;
            result.OutputCount = parent1.OutputCount;
            var chromosomes = new List<Chromosome>();
            for (var i = 0; i < parent1.Chromosomes.Count; i++)
            {
                var dna = CreateNewDna(parent1.Chromosomes[i], parent2.Chromosomes[i]);
                var chromosome = new Chromosome { Dna1 = dna.Dna1, Dna2 = dna.Dna2 };
                chromosomes.Add(chromosome);
            }

            result.Chromosomes = chromosomes;
            return result;
        }

        private (NeuronDna Dna1, NeuronDna Dna2) CreateNewDna(Chromosome parent1Chromosome, Chromosome parent2Chromosome)
        {
            var genesDna1 = new List<BaseGene>();
            var genesDna2 = new List<BaseGene>();
            for (int i = 0; i < parent1Chromosome.Dna1.Genes.Count; i++)
            {
                var parent1Dna = GetRandomBool() ? parent1Chromosome.Dna1 : parent1Chromosome.Dna2;
                var parent2Dna = GetRandomBool() ? parent2Chromosome.Dna1 : parent2Chromosome.Dna2;
                genesDna1.Add(parent1Dna.Genes[i]);
                genesDna2.Add(parent2Dna.Genes[i]);
            }
            var dna1 = new NeuronDna {Genes = genesDna1};
            var dna2 = new NeuronDna {Genes = genesDna2};
            return (dna1, dna2);
        }

        private bool GetRandomBool()
        {
            return _random.NextDouble() > 0.5;
        }
    }
}
