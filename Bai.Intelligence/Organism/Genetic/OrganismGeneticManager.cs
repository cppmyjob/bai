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
        private OrganismGeneticItem[] _men;
        private OrganismGeneticItem[] _women;

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

        public OrganismGeneticItem[] Men => _men;
        public OrganismGeneticItem[] Women => _women;

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
            _men = new OrganismGeneticItem[_initData.ItemsNumber];
            _women = new OrganismGeneticItem[_initData.ItemsNumber];
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

        protected override OrganismGeneticItem CreateItem(IRandom random)
        {
            //var timeMeter = new TimeMeter(_logger, "CreateTime");
            //timeMeter.Start();
            var definition = _networkDefinition.Clone();
            definition.RandomizeValues(random);
            //timeMeter.Stop();

            var result = new OrganismGeneticItem
                         {
                             Definition = definition,
                             Fitness = 0
                         };
            return result;
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

            var newItemsNumber = _initData.ItemsNumber - _initData.SurviveNumber;
            var newMen = new BlockingCollection<OrganismGeneticItem>(newItemsNumber);
            var newWomen = new BlockingCollection<OrganismGeneticItem>(newItemsNumber);
            Parallel.For(0, newItemsNumber, GetParallelOptions(), (i) => {
                var surveyI = Interlocked.Increment(ref gSurveyI) % _initData.SurviveNumber;
                var r = randoms.GetRandom();
                if (r.NextDouble() > 0.5)
                {
                    ReproducePerson(r, crossover, _men, _women, newMen, surveyI);
                    ReproducePerson(r, crossover, _women, _men, newWomen, surveyI);
                }
                else
                {
                    ReproducePerson(r, crossover, _men, _women, newWomen, surveyI);
                    ReproducePerson(r, crossover, _women, _men, newMen, surveyI);
                }
                randoms.Release(r);
            });
            var finalMen = new List<OrganismGeneticItem>(_initData.ItemsNumber);
            finalMen.AddRange(_men.Select(t => t).Take(_initData.SurviveNumber));
            finalMen.AddRange(newMen);
            _men = finalMen.ToArray();
            
            var finalWomen = new List<OrganismGeneticItem>(_initData.ItemsNumber);
            finalWomen.AddRange(_women.Select(t => t).Take(_initData.SurviveNumber));
            finalWomen.AddRange(newWomen);
            _women = finalWomen.ToArray();
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

            //var menList = _men.Where(t => t.Iteration <= 5).OrderByDescending(t => t.Fitness).ToList();
            var menList = _men.OrderByDescending(t => t.Fitness).ToList();

            while (menList.Count < _initData.ItemsNumber)
            {
                var item = CreateItem(random);
                menList.Add(item);
            }

            //var womenList = _women.Where(t => t.Iteration <= 5).OrderByDescending(t => t.Fitness).ToList();
            var womenList = _women.OrderByDescending(t => t.Fitness).ToList();
            while (womenList.Count < _initData.ItemsNumber)
            {
                var item = CreateItem(random);
                womenList.Add(item);
            }

            _men = menList.ToArray();
            _women = womenList.ToArray();

            for (int i = 0; i < 10; i++)
            {
                _logger.Debug($"Max Accuracy Man ({_men[i].Iteration}): {_men[i].Fitness} Woman ({_women[i].Iteration}):{_women[i].Fitness}");
            }

            CreatingPopulation(random, _men, _initData.SurviveNumber);
            CreatingPopulation(random, _women, _initData.SurviveNumber);
        }

        private void CalculateFitness(OrganismGeneticItem person)
        {
            if (person.Fitness < 0.0 || person.Fitness > 0.0)
                return;
            person.Fitness = _fitnessFunction.Calculate(_logger, _inputTrainX, _inputTrainY, person.Definition);
        }

        private void ReproducePerson(IRandom random, Crossover crossover, OrganismGeneticItem[] oldParents1, OrganismGeneticItem[] oldParents2,
            BlockingCollection<OrganismGeneticItem> newPeople, int surveyIndex)
        {
            var parent1 = oldParents1[surveyIndex];
            var parent2Index = random.Next(_initData.ItemsNumber);
            var parent2 = oldParents2[parent2Index];

            var newPersonDefinition = crossover.Execute(parent1.Definition, parent2.Definition);

            newPersonDefinition.Mutate(random);

            var newPerson = new OrganismGeneticItem
            {
                Definition = newPersonDefinition,
                Fitness = 0
            };
            newPeople.Add(newPerson);
        }

    }
}

