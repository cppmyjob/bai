using System;
using System.Collections.Generic;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class CpuRuntime: IRuntime
    {
        public List<Cycle> Cycles { get; } = new List<Cycle>();

        public float[] TempMemory { get; set; }
        public float[] Memory { get; set; }

        public CpuRuntime()
        {

        }


        public float[] Compute(float[] input)
        {
            throw new NotImplementedException();
        }
    }
}
