using System.Collections.Generic;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Organism.Definition
{
    public class NetworkDefinition
    {
        public List<Chromosome> Chromosomes { get; set; }
        public int InputCount { get; set; }
        public int OutputCount { get; set; }

        // TODO Unit tests
        public NetworkDefinition Clone()
        {
            var result = new NetworkDefinition();
            result.InputCount = InputCount;
            result.OutputCount = OutputCount;

            var newChromosomes = new List<Chromosome>(Chromosomes.Count);

            foreach (var chromosome in Chromosomes)
            {
                var newChromosome = chromosome.Clone();
                newChromosomes.Add(newChromosome);
            }

            result.Chromosomes = newChromosomes;

            return result;
        }

        public void RandomizeValues(IRandom random)
        {
            foreach (var chromosome in Chromosomes)
            {
                chromosome.RandomizeValues(random);
            }
        }

        public void Mutate(IRandom random)
        {
            foreach (var chromosome in Chromosomes)
            {
                chromosome.Mutate(random);
            }
        }
    }
}
