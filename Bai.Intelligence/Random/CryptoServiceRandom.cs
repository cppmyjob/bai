using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Random
{
    public class CryptoServiceRandom : IRandom
    {
        private readonly RNGCryptoServiceProvider _random = new RNGCryptoServiceProvider();

        public double NextDouble()
        {
            byte[] b = new byte[4];
            _random.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }

        public int Next(int maxValue)
        {
            return (int)(System.Math.Round(NextDouble() * (maxValue - 1)));
        }

        private void ReleaseUnmanagedResources()
        {
            // TODO release unmanaged resources here
        }

        private void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                _random?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CryptoServiceRandom()
        {
            Dispose(false);
        }
    }
}
