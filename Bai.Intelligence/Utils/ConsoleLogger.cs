using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Utils
{
    public class ConsoleLogger: ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
