using System;
using System.Collections.Generic;
using System.Text;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class MultiCycle : Cycle
    {
        public struct Item
        {
            public float Weight;
            public int SourceIndex;
            public int OutputIndex;
            public int NeuronIndex;
        }

        public List<Item> Items { get; } 

        public MultiCycle(int count)
        {
            Items = new List<Item>(count);
        }

        public override void Compute(float[] tempMemory)
        {
            var length = Items.Count;
            for (var i = 0; i < length; i++)
            {
                var item = Items[i];
                tempMemory[item.OutputIndex] = item.Weight * tempMemory[item.SourceIndex];
            }
        }
    }
}
