using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bai.Intelligence.Collections
{
    public class PList<T>: IDisposable
    {
        private T[][] _buckets;
        private int _count;
        private int _bucketsCount;

        private const int BucketSize = 64;

        public PList() => Init();

        public PList<T> Init()
        {
            _buckets = ArrayPool<T[]>.Shared.Rent(1);
            _buckets[0] = ArrayPool<T>.Shared.Rent(BucketSize);

            _count = 0;
            _bucketsCount = 1;
            return this;
        }

        public void Add(T item)
        {
            var bsi = _count / BucketSize;
            var bi = _count % BucketSize;

            if (bsi == _bucketsCount)
            {
                if (_bucketsCount == _buckets.Length)
                {
                    var newBuckets = ArrayPool<T[]>.Shared.Rent(_buckets.Length * 2);
                    Array.Copy(_buckets, newBuckets, _buckets.Length);
                    ArrayPool<T[]>.Shared.Return(_buckets);
                    _buckets = newBuckets;
                }
                _buckets[_bucketsCount] = ArrayPool<T>.Shared.Rent(BucketSize);
                ++_bucketsCount;
            }

            _buckets[bsi][bi] = item;
            ++_count;
        }

        public void Clear()
        {
            _count = 0;
        }

        public int Count => _count;

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index >= _count)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                var bsi = index / BucketSize;
                var bi = index % BucketSize;
                return _buckets[bsi][bi];

            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (index >= _count)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                var bsi = index / BucketSize;
                var bi = index % BucketSize;
                _buckets[bsi][bi] = value;
            }
        }

        public void Dispose()
        {
            if (_buckets != null)
            {
                for (var i = 0; i < _buckets.Length; i++)
                {
                    var bucket = _buckets[i];
                    if (bucket == null)
                        continue;
                    ArrayPool<T>.Shared.Return(bucket);
                    _buckets[i] = default;
                }

                ArrayPool<T[]>.Shared.Return(_buckets);
                _buckets = default;
            }
        }
    }
}
