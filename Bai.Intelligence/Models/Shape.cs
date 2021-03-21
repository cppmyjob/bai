using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Models
{
    public class Shape
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Shape(int x, int y = 0, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int GetLength()
        {
            var result = X;
            if (Y != 0)
                result *= Y;
            if (Z != 0)
                result *= Z;
            return result;
        }
    }
}
