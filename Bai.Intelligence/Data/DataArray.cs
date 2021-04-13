using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Data
{
    public class DataArray
    {
        private readonly int[] _dim;

        public readonly List<float[]> _data;
        
        public DataArray(params int[] dim)
        {
            _dim = dim;
            _data = new List<float[]>();
        }

        private DataArray(int[] dim, List<float[]> data)
        {
            _dim = dim;
            _data = data;
        }

        public void Add(float[,] data)
        {
            // TODO Check dimensions
            var y = data.GetLength(0);
            var x = data.GetLength(1);
            var count = x * y;
            var d = new float[count];
            System.Buffer.BlockCopy(data, 0, d, 0, count);
            _data.Add(d);
        }

        public void Add(byte[,] data)
        {
            // TODO Check dimensions
            var y = data.GetLength(0);
            var x = data.GetLength(1);
            var count = x * y;
            var floatData = new float[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    floatData[i, j] = (float)data[i, j];
                }
            }
            Add(floatData);
        }

        public void AddRange(List<byte[,]> data)
        {
            foreach (var item in data)
            {
                Add(item);
            }
        }

        public void Add(float[] data)
        {
            _data.Add(data);
        }


        public static DataArray operator /(DataArray a, float value)
        {
            if (value == 0)
            {
                throw new DivideByZeroException();
            }

            var data = new List<float[]>();
            foreach (var row in data)
            {
                var newRow = new float[row.Length];
                for (int i = 0; i < row.Length; i++)
                {
                    newRow[i] = row[i] / value;
                }
            }
            return new DataArray(a._dim, data);
        }
    }
}
