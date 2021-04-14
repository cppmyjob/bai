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

        public void Execute()
        {
            using var random = RandomFactory.Instance.Create();
            InitPopulation(random);
            for (var i = 0; i < _initData.RepeatNumber; i++)
            {
                Process(random);
            }
        }

        protected abstract void InitPopulation(IRandom random);

        protected virtual void CreatingPopulation(IRandom random, TGeneticItem[] items, int from)
        {
            for (var i = from; i < items.Length; i++)
            {
                TGeneticItem item = CreateItem(random);
                items[i] = item;
            }
        }

        protected abstract TGeneticItem CreateItem(IRandom random);

        protected abstract void Reproduction();

        private void Process(IRandom random)
        {
            Reproduction();
            Selection();
        }

        private void Selection()
        {
            
        }

        //private ParallelOptions GetParallelOptions()
        //{
        //    var parallelOption = new ParallelOptions();

        //    if (DataObject.CommandInitData.ProcessorsCount >= 1)
        //    {
        //        parallelOption.MaxDegreeOfParallelism = DataObject.CommandInitData.ProcessorsCount;
        //    }
        //    else
        //    {
        //        parallelOption.MaxDegreeOfParallelism = -1;
        //    }
        //    return parallelOption;
        //}

        //private void Reproduction()
        //{
        //    var gSurveyI = -1;


        //    //Parallel.For(0, DataObject.CommandInitData.ItemsCount - DataObject.CommandInitData.SurviveCount,
        //    //    GetParallelOptions(),
        //    //    (i) =>
        //    //    {
        //    //        var surveyI = Interlocked.Increment(ref gSurveyI) % DataObject.CommandInitData.SurviveCount;

        //    //        var firstParent = _itemsArray[surveyI];
        //    //        //var secondParentIndex = Random.Next(DataObject.CellInitData.SurviveCount);
        //    //        var secondParentIndex = Random.Next(DataObject.CommandInitData.ItemsCount);
        //    //        var secondParent = _itemsArray[secondParentIndex];
        //    //        var child = CreateChild(firstParent, secondParent);
        //    //        if (child != null)
        //    //        {
        //    //            Mutation(child);
        //    //        }
        //    //        else
        //    //        {
        //    //            child = InternalCreateItem();
        //    //            FillValues(child);
        //    //        }
        //    //        _itemsArray[i + DataObject.CommandInitData.SurviveCount] = child;


        //    //    });
        //}
    }
}
