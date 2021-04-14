using System;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition;

namespace Bai.Intelligence.Organism
{
    public class OrganismGeneticManager: GeneticManager<NetworkDefinition>
    {
        private readonly GeneticInitData _initData;
        private readonly NetworkDefinition _networkDefinition;

        private NetworkDefinition[] _men;
        private NetworkDefinition[] _women;

        public OrganismGeneticManager(GeneticInitData initData,
            NetworkDefinition networkDefinition) : base(initData)
        {
            _initData = initData;
            _networkDefinition = networkDefinition;
        }


        protected override void InitPopulation(IRandom random)
        {
            _men = new NetworkDefinition[_initData.ItemsNumber];
            _women = new NetworkDefinition[_initData.ItemsNumber];
            if (_initData.Men != null)
            {
                Array.Copy(_initData.Men, _men, _initData.Men.Length);
                CreatingPopulation(random, _men, _initData.Men.Length);
            }
            else
                CreatingPopulation(random, _men, 0);

            if (_initData.Women != null)
            {
                Array.Copy(_initData.Women, _women, _initData.Women.Length);
                CreatingPopulation(random, _women, _initData.Women.Length);
            }
            else
                CreatingPopulation(random, _women, 0);
        }

        protected override NetworkDefinition CreateItem(IRandom random)
        {
            var result = _networkDefinition.Clone();
            result.RandomizeValues(random);
            return result;
        }

        protected override void Reproduction()
        {
            throw new NotImplementedException();
        }
    }
}
