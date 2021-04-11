using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Function
{
    public class LinearFunction : INeuronFunctionOneToOne
    {
        public float Compute(float value)
        {
            return value;
        }
    }
}
