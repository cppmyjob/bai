using System;

namespace Bai.Intelligence.Function
{
    public class SigmoidFunction : INeuronFunction
    {
        public float Alfa { get; set; }
        public float Compute(float value)
        {
            return (float) (1.0 / (1.0 + Math.Exp(-Alfa * value)));
        }
    }
}
