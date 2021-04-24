using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bai.Intelligence.Data;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Utils;
using Bai.Intelligence.Utils.Random;

namespace Bai.Intelligence.Organism.Genetic
{

    public class OrganismGeneticItem
    {
        public NetworkDefinition Definition;
        public double Fitness;
        public int Iteration;
        public bool Died;

        public void Die()
        {
            Died = true;
        }

        public void Born()
        {
            Fitness = 0;
            Iteration = 0;
            Died = false;
        }
    }

    public class OrganismGeneticManager: GeneticManager<OrganismGeneticItem>
    {
        private readonly ILogger _logger;
        private readonly GeneticInitData _initData;
        private readonly NetworkDefinition _networkDefinition;
        private readonly IFitnessFunction<NetworkDefinition> _fitnessFunction;
        private readonly DataArray _trainY;
        private readonly DataArray _trainX;
        private InputDataArray _inputTrainY;
        private InputDataArray _inputTrainX;


        // TODO use List
        private List<OrganismGeneticItem> _men;
        private List<OrganismGeneticItem> _women;

        public OrganismGeneticManager(ILogger logger, GeneticInitData initData,
            NetworkDefinition networkDefinition, DataArray trainX, DataArray trainY,
            IFitnessFunction<NetworkDefinition> fitnessFunction) : base(initData)
        {
            _logger = logger;
            _initData = initData;
            _networkDefinition = networkDefinition;
            _trainY = trainY;
            _fitnessFunction = fitnessFunction;
            _trainX = trainX;
        }

        public List<OrganismGeneticItem> Men => _men;
        public List<OrganismGeneticItem> Women => _women;

        public override void Execute()
        {
            var dim = _trainY.GetDimension();
            if (dim.Length > 1)
                throw new Exception("Fitness supports 1D array for trainY only");

            if (dim[0] != _networkDefinition.OutputCount)
            {
                throw new Exception("trainY != network output count");
            }

            _inputTrainX = new InputDataArray(_trainX);
            _inputTrainY = new InputDataArray(_trainY);

            if (_inputTrainX.FrameLength != _networkDefinition.InputCount)
                throw new Exception($"Invalid input count expected {_networkDefinition.InputCount} but was {_inputTrainX.FrameLength}");

            base.Execute();
        }

        protected override void InitPopulation(IRandom random)
        {
            _men = new List<OrganismGeneticItem>(_initData.ItemsNumber);
            _women = new List<OrganismGeneticItem>(_initData.ItemsNumber);

            for (var i = 0; i < _initData.ItemsNumber; i++)
            {
                _men.Add(null);
                _women.Add(null);
            }

            if (_initData.Men != null)
            {
                throw new NotImplementedException();
                //Array.Copy(_initData.Men, _men, _initData.Men.Length);
                //CreatingPopulation(random, _men, _initData.Men.Length);
            }
            else
                CreatingPopulation(random, _men, 0);

            if (_initData.Women != null)
            {
                throw new NotImplementedException();
                //Array.Copy(_initData.Women, _women, _initData.Women.Length);
                //CreatingPopulation(random, _women, _initData.Women.Length);
            }
            else
                CreatingPopulation(random, _women, 0);
        }

        protected override OrganismGeneticItem CreateItem(OrganismGeneticItem diedItem, IRandom random)
        {
            if (diedItem == null)
            {
                var timeMeter = new TimeMeter(_logger, "CreateTime");
                timeMeter.Start();
                var definition = _networkDefinition.Clone();
                definition.RandomizeValues(random);
                timeMeter.Stop();
                var result = new OrganismGeneticItem
                             {
                                 Definition = definition,
                                 Fitness = 0
                             };
                return result;
            }
            else
            {
                //diedItem.Born();
                diedItem.Definition.RandomizeValues(random);
                return diedItem;
            }
        }

        protected override void Reproduction(IRandom random)
        {
            using var randoms = new Randoms();
            var gSurveyI = -1;
            var crossover = new Crossover(random);

            Parallel.For(0, _initData.SurviveNumber, GetParallelOptions(), (i) =>
            {
                _men[i].Iteration = _men[i].Iteration + 1;
                _women[i].Iteration = _women[i].Iteration + 1;
            });

            Parallel.For(_initData.SurviveNumber, _initData.ItemsNumber, GetParallelOptions(), (i) =>
            {
                var surveyI = Interlocked.Increment(ref gSurveyI) % _initData.SurviveNumber;
                var r = randoms.GetRandom();
                if (r.NextDouble() > 0.5)
                {
                    ReproducePerson(r, crossover, _men, _women, _men[i], surveyI);
                    ReproducePerson(r, crossover, _women, _men, _women[i], surveyI);
                }
                else
                {
                    ReproducePerson(r, crossover, _men, _women, _women[i], surveyI);
                    ReproducePerson(r, crossover, _women, _men, _men[i], surveyI);
                }
                randoms.Release(r);
            });
        }

        protected override void Selection(IRandom random)
        {
            Parallel.For(0, _initData.ItemsNumber * 2, GetParallelOptions(), (i) => {
                if (i < _initData.ItemsNumber) {
                    CalculateFitness(_men[i]);
                }
                else
                {
                    CalculateFitness(_women[i - _initData.ItemsNumber]);
                }
            });


            _men.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));
            _women.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

            for (int i = 0; i < 10; i++)
            {
                _logger.Debug($"Max Accuracy Man ({_men[i].Iteration}): {_men[i].Fitness} Woman ({_women[i].Iteration}):{_women[i].Fitness}");
            }

            DieItems(_men, _initData.SurviveNumber);
            DieItems(_women, _initData.SurviveNumber);

            CreatingPopulation(random, _men, _initData.SurviveNumber);
            CreatingPopulation(random, _women, _initData.SurviveNumber);
        }


        private void DieItems(List<OrganismGeneticItem> items, int from)
        {
            Parallel.For(from, items.Count, GetParallelOptions(), (i) =>
            {
                items[i].Die();
            });
        }

        private void CalculateFitness(OrganismGeneticItem person)
        {
            if (person.Fitness < 0.0 || person.Fitness > 0.0)
                return;
            person.Fitness = _fitnessFunction.Calculate(_logger, _inputTrainX, _inputTrainY, person.Definition);
        }

        private void ReproducePerson(IRandom random, Crossover crossover, List<OrganismGeneticItem> oldParents1, List<OrganismGeneticItem> oldParents2,
            OrganismGeneticItem blank, int surveyIndex)
        {
            var parent1 = oldParents1[surveyIndex];
            var parent2Index = random.Next(_initData.ItemsNumber);
            var parent2 = oldParents2[parent2Index];

            crossover.Execute(parent1.Definition, parent2.Definition, blank.Definition);

            blank.Definition.Mutate(random);

            blank.Born();
        }

    }
}

