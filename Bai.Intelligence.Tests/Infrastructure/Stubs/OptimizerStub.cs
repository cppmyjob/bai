using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Models.Optimizers;
using Bai.Intelligence.Organism.Definition;

namespace Bai.Intelligence.Tests.Infrastructure.Stubs
{
    public class OptimizerStub : Optimizer
    {
        public override void Run(NetworkDefinition networkDefinition)
        {
            throw new NotImplementedException();
        }
    }
}
