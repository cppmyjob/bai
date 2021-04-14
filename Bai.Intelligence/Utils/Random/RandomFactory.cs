using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Utils.Random
{
    public class RandomFactory : IRandomFactory
    {
        public static IRandomFactory Instance { get; set; } = new RandomFactory();

        public IRandom Create()
        {
            return new CryptoServiceRandom();
        }
    }
}
