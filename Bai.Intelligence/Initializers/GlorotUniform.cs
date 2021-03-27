using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Random;

namespace Bai.Intelligence.Initializers
{
    // https://keras.io/api/layers/initializers/
    public class GlorotUniform
    {
        public float[] GetValues(int fanIn, int fanOut)
        {
            var count = fanIn * fanOut;
            var limit = Math.Sqrt(6.0 / (fanIn + fanOut));
            var result = new float[count];

            using var random = RandomFactory.Instance.Create();
            for (var i = 0; i < count; i++)
            {
                result[i] = (float) (random.NextDouble() * 2.0 * limit - limit);
            }

            return result;
        }
    }
}
