using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Utils.Random
{
    public class Randoms : IDisposable
    {
        private const int Count = 32 * 3;

        private readonly int _count;
        private readonly IRandom[] _randoms;
        private readonly ConcurrentStack<IRandom> _stack = new ConcurrentStack<IRandom>();

        public Randoms(int count = Count)
        {
            _count = count;
            _randoms = new IRandom[count];
            for (var i = 0; i < _randoms.Length; i++)
            {
                _randoms[i] = RandomFactory.Instance.Create();
                _stack.Push(_randoms[i]);
            }
        }

        // TODO GetRandom / Release improve
        public IRandom GetRandom()
        {
            if (_stack.TryPop(out var result))
                return result;
            throw new Exception("Randoms is empty");
        }

        public void Release(IRandom random)
        {
            _stack.Push(random);
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
