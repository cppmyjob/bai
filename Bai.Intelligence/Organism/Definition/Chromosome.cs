using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition.Dna;

namespace Bai.Intelligence.Organism.Definition
{
    public class Chromosome
    {
        public NeuronDna Dna1 { get; set; }
        public NeuronDna Dna2 { get; set; }

        public Chromosome Clone()
        {
            var result = new Chromosome();
            result.Dna1 = Dna1.Clone();
            result.Dna2 = Dna2.Clone();
            return result;
        }

        public void RandomizeValues(IRandom random)
        {
            Dna1.RandomizeValues(random);
            Dna2.RandomizeValues(random);
        }

        public void Mutate(IRandom random)
        {
            Dna1.Mutate(random);
            Dna2.Mutate(random);
        }
    }
}
