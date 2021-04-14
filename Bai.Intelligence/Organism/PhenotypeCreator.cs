using System.Collections.Generic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna;
using Bai.Intelligence.Organism.Definition.Dna.Genes;

namespace Bai.Intelligence.Organism
{
    public class PhenotypeCreator
    {
        private readonly IRandom _random;

        public PhenotypeCreator(IRandom random)
        {
            _random = random;
        }

        // TODO Пока не буду учитывать что ген может быть рецесивным, на основании индексов Input и Output
        // Нужно подумать можно ли это делать на основании весов и других полей генов
        // Таким образом все гены становятся доминантыми
        // см. http://www.chemport.ru/forum/viewtopic.php?t=91372
        // https://yandex.ru/q/question/science/kak_organizm_raspoznaet_kakoi_gen_a_kakoi_e1f28a8e/

        public List<NeuronDna> Execute(NetworkDefinition definition)
        {
            var result = new List<NeuronDna>();

            foreach (var chromosome in definition.Chromosomes)
            {
                var woman = chromosome.Dna2;
                var man = chromosome.Dna1;

                var phenotypeGenes = new List<BaseGene>(woman.Genes.Count);
                for (var j = 0; j < woman.Genes.Count; j++)
                {
                    var womanGene = woman.Genes[j];
                    var manGene = man.Genes[j];
                    var gene = GetFinalGene(womanGene, manGene);
                    phenotypeGenes.Add(gene);
                }
                var phenotypeDna = new NeuronDna();
                phenotypeDna.Genes = phenotypeGenes;
                result.Add(phenotypeDna);
            }

            return result;
        }

        // Сейчас будем определять фенотип, на основании равновероятностного использования генов из разных аллелей
        // Дальше планирую комбинацию из полей генов
        private BaseGene GetFinalGene(BaseGene womanGene, BaseGene manGene)
        {
            return _random.NextDouble() > 0.5 ? manGene : womanGene;
        }
    }
}
