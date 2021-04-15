using System;
using System.Collections.Generic;
using Bai.Intelligence.Data;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class CpuRuntime: IRuntime
    {
        private readonly int _inputCount;
        private readonly int _outputCount;
        public List<Cycle> Cycles { get; } = new List<Cycle>();

        public float[] TempMemory { get; set; }

        public float[] InputMemory { get; private set; }

        public CpuRuntime(int inputCount, int outputCount)
        {
            _inputCount = inputCount;
            _outputCount = outputCount;
        }

        public void SetInputMemory(float[] inputMemory)
        {
            InputMemory = inputMemory;
        }

        public float[] Compute(InputData[] inputs)
        {
            var offset = 0;
            foreach (var input in inputs)
            {
                Array.Copy(InputMemory, input.Offset, TempMemory, offset, input.Length);
                offset += input.Length;
            }

            foreach (var cycle in Cycles)
            {
                cycle.Compute(TempMemory);
            }

            var result = new float[_outputCount];
            Array.Copy(TempMemory, _inputCount, result, 0, _outputCount);
            return result;
        }
    }
}
