using System;
using Bai.Intelligence.Data;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Genetic;

namespace Bai.Intelligence.Models.Optimizers
{
    public class GeneticOptimizer: Optimizer
    {
        public override void Run(ILogger logger, NetworkDefinition networkDefinition, DataArray x, DataArray y)
        {
            var initData = new GeneticInitData(32, 10, 10, -1);
            var manager = new OrganismGeneticManager(logger, initData, networkDefinition, x, y,
                new AccuracyFitnessFunction());
            manager.Execute();

            logger.Debug($"Accuracy Man:{manager.Men[0].Fitness} Woman:{manager.Women[0].Fitness}");
        }

    }
}
