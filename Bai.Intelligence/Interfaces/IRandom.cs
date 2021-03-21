using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Interfaces
{
    public interface IRandom : IDisposable
    {
        double NextDouble();

        int Next(int maxValue);
    }
}
