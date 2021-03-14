using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Function;

namespace Bai.Intelligence.Tests.Infrastructure.Stubs
{
    public class NeuronFunctionStub: INeuronFunction
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
