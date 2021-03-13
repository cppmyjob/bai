using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class MultiCycle : Cycle
    {
        public class Item
        {
            public float Weight { get; set; }
            public int SourceIndex { get; set; }
            public int OutputIndex { get; set; }
            public int NeuronIndex { get; set; }
        }

        public List<Item> Items { get; } 

        public MultiCycle(int count)
        {
            Items = new List<Item>(count);
        }

        public override void Compute(float[] memory, float[] tempMemory)
        {
            throw new NotImplementedException();
        }
    }
}
