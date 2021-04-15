using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bai.Intelligence.Data
{
    public class InputDataArray
    {
        public InputDataArray(DataArray dataArray)
        {
            Data = dataArray.SelectMany(t => t).ToArray();
            FrameLength = dataArray.GetDimension().Aggregate(1, (x, y) => x * y);
        }

        public float[] Data { get; set; }
        public int FrameLength { get; }
    }
}
