using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Organism.Functions;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class FunctionManyToManyCycle : Cycle
    {
        public class Item
        {
            public INeuronFunctionManyToMany Function { get; set; }
            public int[] InputValueIndexes { get; set; }
            public int[] TempOutputIndexes { get; set; }
        }

        public List<Item> Items { get; }

        public FunctionManyToManyCycle(int count)
        {
            Items = new List<Item>(count);
        }

        public override void Compute(float[] tempMemory)
        {
            foreach (var item in Items)
            {
                var values = new float[item.InputValueIndexes.Length];
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = tempMemory[item.InputValueIndexes[i]];
                }

                var results = item.Function.Compute(values);

                for (var i = 0; i < results.Length; i++)
                {
                    tempMemory[item.TempOutputIndexes[i]] = results[i];
                }
            }
        }
    }
}
