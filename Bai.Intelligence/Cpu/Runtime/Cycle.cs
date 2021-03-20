using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Cpu.Runtime
{
    public abstract class Cycle
    {
        public abstract void Compute(float[] tempMemory);
    }
}
