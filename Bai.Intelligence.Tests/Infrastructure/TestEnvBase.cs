using Bai.Intelligence.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai.Intelligence.Tests.Infrastructure
{
    public class TestEnvBase
    {
        public NetworkDefinition CreateSimpleNeuron()
        {
            var cell = new NetworkDefinition();

            var chromosome = new Chromosome();
            cell.Chromosomes = new[] {chromosome};
            return cell;
        }
    }
}
