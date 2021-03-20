using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Interfaces
{

    public class RuntimeInput
    {
        public int Offset { get; set; }
        public int Length { get; set; }
    }

    public interface IRuntime
    {
        float[] Compute(RuntimeInput[] inputs);
    }
}
