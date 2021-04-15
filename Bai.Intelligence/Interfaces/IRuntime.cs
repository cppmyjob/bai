using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Data;

namespace Bai.Intelligence.Interfaces
{

    public interface IRuntime
    {
        void SetInputMemory(float[] inputMemory);
        float[] Compute(InputData[] inputs);
    }
}
