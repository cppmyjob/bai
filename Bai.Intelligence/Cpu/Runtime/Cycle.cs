using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Cpu.Runtime
{
    public abstract class Cycle : IDisposable
    {
        public abstract void Compute(float[] tempMemory);

        public abstract void Dispose();


    }
}
