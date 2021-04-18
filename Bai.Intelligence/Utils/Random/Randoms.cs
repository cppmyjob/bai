using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Utils.Random
{
    public class Randoms : IDisposable
    {
        private const int Count = 32;

        private readonly int _count;
        private readonly IRandom[] _randoms;
        public Randoms(int count = Count)
        {
            _count = count;
            _randoms = new IRandom[count];
            for (var i = 0; i < _randoms.Length; i++)
            {
                _randoms[i] = RandomFactory.Instance.Create();
            }
        }

        public IRandom GetRandom(int i)
        {
            return _randoms[i % _count];
        }

        public void Dispose()
        {
            for (var i = 0; i < _randoms.Length; i++)
            {
                _randoms[i].Dispose();
            }
        }
    }
}
