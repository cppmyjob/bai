using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Organism.Functions;

namespace Bai.Intelligence.Tests.Infrastructure.Stubs
{
    public class NeuronFunctionOneToOneStub: INeuronFunctionOneToOne
    {
        public static int CallCount { get; set; } = 0;
        public static float InputValue { get; set; } = 0;

        public float Compute(float value)
        {
            ++CallCount;
            InputValue = value;
            return 55F;
        }
    }
}
