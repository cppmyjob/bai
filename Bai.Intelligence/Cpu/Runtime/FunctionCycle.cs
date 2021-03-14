using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Function;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class FunctionCycle: Cycle
    {
        public class Item
        {
            public INeuronFunction Function { get; set; }
            public int InputValueIndex { get; set; }
            public int OutputIndex { get; set; }
        }

        public List<Item> Items { get; }

        public FunctionCycle(int count)
        {
            Items = new List<Item>(count);
        }

        public override void Compute(float[] memory, float[] tempMemory)
        {
            foreach (var item in Items)
            {
                tempMemory[item.OutputIndex] = item.Function.Compute(tempMemory[item.InputValueIndex]);
            }
        }
    }
}
