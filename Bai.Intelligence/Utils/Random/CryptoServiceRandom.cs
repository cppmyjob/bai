using System;
using System.Security.Cryptography;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Utils.Random
{
    public class CryptoServiceRandom : IRandom
    {
        private readonly RNGCryptoServiceProvider _random = new();
        private readonly System.Random _lightRandom;

        public CryptoServiceRandom()
        {
            var b = new byte[4];
            _random.GetBytes(b);
            _lightRandom = new System.Random(BitConverter.ToInt32(b, 0));
        }

        public double NextDouble()
        {
            //byte[] b = new byte[4];
            //_random.GetBytes(b);
            //return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
            lock (_lightRandom)
            {
                return _lightRandom.NextDouble();
            }
        }

        public int Next(int maxValue)
        {
            //return (int)(System.Math.Round(NextDouble() * (maxValue - 1)));
            lock (_lightRandom)
            {
                return _lightRandom.Next(maxValue);
            }
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
