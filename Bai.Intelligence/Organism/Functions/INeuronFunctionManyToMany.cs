namespace Bai.Intelligence.Organism.Functions
{
    public interface INeuronFunctionManyToMany : INeuronFunction
    {
        float[] Compute(float[] values);
    }
}
