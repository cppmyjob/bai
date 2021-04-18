namespace Bai.Intelligence.Organism.Functions
{
    public class LinearFunction : INeuronFunctionOneToOne
    {
        public float Compute(float value)
        {
            return value;
        }

        public FunctionIoType FunctionIoType => FunctionIoType.OneToOne;
    }
}
