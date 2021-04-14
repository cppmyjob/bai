using System;

namespace Bai.Intelligence.Organism.Functions
{
    // https://towardsdatascience.com/softmax-activation-function-explained-a7e1bc3ad60
    // http://rinterested.github.io/statistics/softmax.html
    // https://www.tensorflow.org/api_docs/python/tf/keras/layers/Softmax
    // https://www.youtube.com/watch?v=omz_NdFgWyU см код в конце
    public class SoftMaxFunction : INeuronFunctionManyToMany
    {
        public float[] Compute(float[] values)
        {
            double sum = 0.0;
            var exps = new double[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                exps[i] = Math.Exp(values[i]);
                sum += exps[i];
            }

            var result = new float[values.Length];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = (float)(exps[i] / sum);
            }
            return result;
        }
    }
}
