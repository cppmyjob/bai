using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Data
{
    public class DataSet<TX, TY>
    {
        public TX X { get; }
        public TY Y { get; }

        public DataSet(TX x, TY y)
        {
            X = x;
            Y = y;
        }
    }
}
