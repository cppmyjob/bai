using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Random
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
