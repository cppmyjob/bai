using System;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism;
using Bai.Intelligence.Organism.Definition;

namespace Bai.Intelligence.Models.Optimizers
{
    public class GeneticOptimizer: Optimizer
    {
        public override void Run(ILogger logger, NetworkDefinition networkDefinition)
        {
            var initData = new GeneticInitData(1000, 300, 100, 16);
            var manager = new OrganismGeneticManager(initData, networkDefinition);
            manager.Execute();
        }

    }
}
