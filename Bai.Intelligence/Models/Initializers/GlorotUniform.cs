using System;
using Bai.Intelligence.Utils.Random;

namespace Bai.Intelligence.Models.Initializers
{
    // https://keras.io/api/layers/initializers/
    public class GlorotUniform
    {
        public InitializerResult GetValues(int fanIn, int fanOut)
        {
            var count = fanIn * fanOut;
            var limit = Math.Sqrt(6.0 / (fanIn + fanOut));
            var weights = new float[count];

            using var random = RandomFactory.Instance.Create();
            for (var i = 0; i < count; i++)
            {
                weights[i] = (float) (random.NextDouble() * 2.0 * limit - limit);
            }

            var result = new InitializerResult();
            result.Weights = weights;
            result.MinWeight = -(float) limit;
            result.MaxWeight = (float)limit;
            return result;
        }
    }
}
