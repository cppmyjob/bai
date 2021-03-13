using System;
using System.Collections.Generic;
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

        public override void Compute(float[] memory, float[] tempMemory)
        {
            throw new NotImplementedException();
        }
    }
}
