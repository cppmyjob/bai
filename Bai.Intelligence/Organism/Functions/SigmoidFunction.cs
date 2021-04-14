using System;

namespace Bai.Intelligence.Organism.Functions
{
    public class SigmoidFunction : INeuronFunctionOneToOne
    {
        public float Alfa { get; set; }
        public float Compute(float value)
        {
            return (float) (1.0 / (1.0 + Math.Exp(-Alfa * value)));
        }
    }
}
