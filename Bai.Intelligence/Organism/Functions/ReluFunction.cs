using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Organism.Functions
{
    public class ReluFunction: INeuronFunctionOneToOne
    {
        public float Compute(float value)
        {
            return Math.Max(0, value);
        }

        public FunctionIoType FunctionIoType => FunctionIoType.OneToOne;
    }
}
