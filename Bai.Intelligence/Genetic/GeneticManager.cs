using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Utils.Random;

namespace Bai.Intelligence.Genetic
{
    public abstract class GeneticManager<TGeneticItem>
    {
        private readonly GeneticInitData _initData;

        protected GeneticManager(GeneticInitData initData)
        {
            _initData = initData;
        }

        public virtual void Execute()
        {
            using var random = RandomFactory.Instance.Create();
            InitPopulation(random);
            for (var i = 0; i < _initData.RepeatNumber; i++)
            {
                using var random2 = RandomFactory.Instance.Create();
                Process(random2);
            }
        }

        protected abstract void InitPopulation(IRandom random);

        protected virtual void CreatingPopulation(IRandom random, List<TGeneticItem> items, int from)
        {
            using var randoms = new Randoms();
            Parallel.For(from, items.Count, GetParallelOptions(), (i) =>
            {
                var r = randoms.GetRandom();
                items[i] = CreateItem(items[i], r);
                randoms.Release(r);
            });
        }

        protected abstract TGeneticItem CreateItem(TGeneticItem diedItem, IRandom random);

        protected abstract void Reproduction(IRandom random);
        protected abstract void Selection(IRandom random);

        protected ParallelOptions GetParallelOptions()
        {
            var parallelOption = new ParallelOptions();

            if (_initData.ProcessorCoreNumber >= 1)
            {
                parallelOption.MaxDegreeOfParallelism = _initData.ProcessorCoreNumber;
            }
            else
            {
                parallelOption.MaxDegreeOfParallelism = -1;
            }
            return parallelOption;
        }

        private void Process(IRandom random)
        {
            Reproduction(random);
            Selection(random);
        }
    }
}
