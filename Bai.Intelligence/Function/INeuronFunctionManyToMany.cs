namespace Bai.Intelligence.Function
{
    public interface INeuronFunctionManyToMany : INeuronFunction
    {
        float[] Compute(float[] values);
    }
}
