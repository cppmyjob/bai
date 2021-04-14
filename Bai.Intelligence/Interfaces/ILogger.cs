using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Interfaces
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
    }
}
