using System;
using System.Collections.Generic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition.Dna.Genes;

namespace Bai.Intelligence.Organism.Definition.Dna
{
    public class NeuronDna
    {
        public List<BaseGene> Genes { get; set; }

        public NeuronDna Clone()
        {
            var result = new NeuronDna();

            var newGenes = new List<BaseGene>(Genes.Count);
            foreach (var gene in Genes)
            {
                newGenes.Add(gene.Clone());
            }

            result.Genes = newGenes;
            return result;
        }

        public void RandomizeValues(IRandom random)
        {
            foreach (var gene in Genes)
            {
                gene.RandomizeValues(random);
            }
        }

        public void Mutate(IRandom random)
        {
            foreach (var gene in Genes)
            {
                gene.Mutate(random);
            }
        }
    }
}
