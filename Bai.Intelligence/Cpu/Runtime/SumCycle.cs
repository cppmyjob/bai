using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class SumCycle : Cycle
    {
        public struct Item
        {
            public int[] Indexes;
            public int NeuronIndex;
            public int ResultIndex;
        }

        public List<Item> Items { get; }

        public SumCycle(int count)
        {
            Items = new List<Item>(count);
        }

        public override void Compute(float[] tempMemory)
        {
            var lengthI = Items.Count;
            for (var i = 0; i < lengthI; i++)
            {
                var item = Items[i];
                double sum = 0;
                var lengthJ = item.Indexes.Length;
                for (var j = 0; j < lengthJ; j++)
                {
                    sum += tempMemory[item.Indexes[j]];
                }
                tempMemory[item.ResultIndex] = (float)sum;
            }
        }

        public override void Dispose()
        {
        }
    }
}
