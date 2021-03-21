using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bai.Intelligence.Definition;
using Bai.Intelligence.Random;

namespace Bai.Intelligence.Genetic
{
    public class GeneticManager
    {
        private readonly GeneticInitData _initData;

        private NetworkDefinition[] _men;
        private NetworkDefinition[] _women;

        public GeneticManager(GeneticInitData initData)
        {
            _initData = initData;
        }

        public void Execute()
        {
            InitPopulation();
            for (int i = 0; i < _initData.RepeatNumber; i++)
            {
                Process();
            }
        }

        private void InitPopulation()
        {
            _men = new NetworkDefinition[_initData.ItemsNumber];
            _women = new NetworkDefinition[_initData.ItemsNumber];
            if (_initData.Men != null)
            {
                Array.Copy(_initData.Men, _men, _initData.Men.Length);
                CreatingPopulation(_men, _initData.Men.Length);
            }
            else
                CreatingPopulation(_men, 0);

            if (_initData.Women != null)
            {
                Array.Copy(_initData.Women, _women, _initData.Women.Length);
                CreatingPopulation(_women, _initData.Women.Length);
            }
            else
                CreatingPopulation(_women, 0);
        }

        private void CreatingPopulation(NetworkDefinition[] men, int from)
        {
            for (int i = from; i < men.Length; i++)
            {
                //var item = InternalCreateItem();
                //FillValues(item);
                //men[i] = item;
            }
        }

        private void Process()
        {
            using var random = RandomFactory.Instance.Create();
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

        private void Reproduction()
        {
            var gSurveyI = -1;


            //Parallel.For(0, DataObject.CommandInitData.ItemsCount - DataObject.CommandInitData.SurviveCount,
            //    GetParallelOptions(),
            //    (i) =>
            //    {
            //        var surveyI = Interlocked.Increment(ref gSurveyI) % DataObject.CommandInitData.SurviveCount;

            //        var firstParent = _itemsArray[surveyI];
            //        //var secondParentIndex = Random.Next(DataObject.CellInitData.SurviveCount);
            //        var secondParentIndex = Random.Next(DataObject.CommandInitData.ItemsCount);
            //        var secondParent = _itemsArray[secondParentIndex];
            //        var child = CreateChild(firstParent, secondParent);
            //        if (child != null)
            //        {
            //            Mutation(child);
            //        }
            //        else
            //        {
            //            child = InternalCreateItem();
            //            FillValues(child);
            //        }
            //        _itemsArray[i + DataObject.CommandInitData.SurviveCount] = child;


            //    });
        }
    }
}
