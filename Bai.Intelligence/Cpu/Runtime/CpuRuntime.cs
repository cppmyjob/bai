using System;
using System.Buffers;
using System.Collections.Generic;
using Bai.Intelligence.Data;
using Bai.Intelligence.Interfaces;
using MemoryPools;
using MemoryPools.Collections.Specialized;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class CpuRuntime: IRuntime
    {
        private readonly int _inputCount;
        private readonly int _outputCount;
        private float[] _computeResult;
        private float[] _tempMemory;

        public PoolingList<Cycle> Cycles { get; private set; }

        public CpuRuntime(int inputCount, int outputCount)
        {
            _inputCount = inputCount;
            _outputCount = outputCount;
            _computeResult = ArrayPool<float>.Shared.Rent(outputCount);
            Cycles = Pool<PoolingList<Cycle>>.Get().Init();
            
        }

        public int TempMemoryLength { get; private set; }

        internal void SetTempMemory(int size)
        {
            if (_tempMemory != null)
                ArrayPool<float>.Shared.Return(_tempMemory);
            TempMemoryLength = size;
            _tempMemory = ArrayPool<float>.Shared.Rent(size);
        }

        public ReadOnlySpan<float> Compute(float[] inputMemory, InputData[] inputs)
        {
            var offset = 0;
            foreach (var input in inputs)
            {
                Array.Copy(inputMemory, input.Offset, _tempMemory, offset, input.Length);
                offset += input.Length;
            }

            foreach (var cycle in Cycles)
            {
                cycle.Compute(_tempMemory);
            }

            Array.Copy(_tempMemory, _inputCount, _computeResult, 0, _outputCount);
            return new ReadOnlySpan<float>(_computeResult, 0, _outputCount); ;
        }

        public void Dispose()
        {
            if (_computeResult != null)
            {
                ArrayPool<float>.Shared.Return(_computeResult);
                _computeResult = default;
            }

            if (_tempMemory != null)
            {
                ArrayPool<float>.Shared.Return(_tempMemory);
                _tempMemory = default;
            }

            if (Cycles != null)
            {
                for (int i = 0; i < Cycles.Count; i++)
                {
                    Cycles[i].Dispose();
                }

                Cycles.Dispose();
                Pool<PoolingList<Cycle>>.Return(Cycles);
                Cycles = default;
            }
        }
    }
}
