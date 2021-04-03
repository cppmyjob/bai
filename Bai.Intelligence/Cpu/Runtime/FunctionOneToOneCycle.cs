using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Function;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class FunctionOneToOneCycle: Cycle
    {
        public class Item
        {
            public INeuronFunctionOneToOne Function { get; set; }
            public int InputValueIndex { get; set; }
            public int TempOutputIndex { get; set; }
        }

        public List<Item> Items { get; }

        public FunctionOneToOneCycle(int count)
        {
            Items = new List<Item>(count);
        }

        public override void Compute(float[] tempMemory)
        {
            foreach (var item in Items)
            {
                tempMemory[item.TempOutputIndex] = item.Function.Compute(tempMemory[item.InputValueIndex]);
            }
        }
    }
}
