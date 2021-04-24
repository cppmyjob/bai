using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Data;

namespace Bai.Intelligence.Interfaces
{

    public interface IRuntime : IDisposable
    {
        ReadOnlySpan<float> Compute(float[] inputMemory, InputData[] inputs);
    }
}
