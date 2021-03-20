using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class SumCycle : Cycle
    {
        public class Item
        {
            public int[] Indexes { get; set; }
            public int NeuronIndex { get; set; }
            public int ResultIndex { get; set; }
        }

        public List<Item> Items { get; }

        public SumCycle(int count)
        {
            Items = new List<Item>(count);
        }

        public override void Compute(float[] tempMemory)
        {
            foreach (var item in Items)
            {
                var sum = item.Indexes.Sum(t => tempMemory[t]);
                tempMemory[item.ResultIndex] = sum;
            }
        }
    }
}
