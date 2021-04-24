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

        public void CopyTo(NeuronDna value)
        {
            if (value.Genes.Count != Genes.Count)
                throw new Exception("NeuronDna::CopyTo different genes count");

            for (int i = 0; i < Genes.Count; i++)
            {
                Genes[i].CopyTo(value.Genes[i]);
            }
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
