using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Organism.Functions;

namespace Bai.Intelligence.Tests.Infrastructure.Stubs
{
    public class NeuronFunctionManyToManyStub : INeuronFunctionManyToMany
    {
        public static int CallCount { get; set; } = 0;
        public static float[] InputValues { get; set; }

        public float[] Compute(float[] values)
        {
            ++CallCount;
            InputValues = values;
            return new []{ 55F, 66F };
        }

        public FunctionIoType FunctionIoType => FunctionIoType.ManyToMany;
    }
}
