using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Function
{
    // https://towardsdatascience.com/softmax-activation-function-explained-a7e1bc3ad60
    // http://rinterested.github.io/statistics/softmax.html
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
