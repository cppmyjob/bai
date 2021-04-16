using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

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

        private float[] _weights;
        private float[] _tempMemory;
        private int[] _outputIndexes;
        private int[] _sourceIndexes;

        public static int Count = 0;

        //[Intrinsic]
        public override void Compute(float[] tempMemory)
        {
            var length = Items.Count;
            Count += length;
            //if (_weights == null)
            //{
            //    _tempMemory = new float[length];
            //    _weights = new float[length];
            //    _outputIndexes = new int[length];
            //    _sourceIndexes = new int[length];
            //    for (var i = 0; i < length; i++)
            //    {
            //        var item = Items[i];
            //        _weights[i] = item.Weight;
            //        _outputIndexes[i] = item.OutputIndex;
            //        _sourceIndexes[i] = item.SourceIndex;
            //    }
            //}
            //Count += length;
            //for (var i = 0; i < length; i++)
            //{
            //    //tempMemory[_outputIndexes[i]] = _weights[i] * tempMemory[_sourceIndexes[i]];
            //    _tempMemory[i] = _weights[i] * tempMemory[_sourceIndexes[i]];
            //}

            //for (int i = 0; i < length; i++)
            //{
            //    tempMemory[_outputIndexes[i]] = _tempMemory[i];
            //}

            for (var i = 0; i < length; i++)
            {
                var item = Items[i];
                tempMemory[item.OutputIndex] = item.Weight * tempMemory[item.SourceIndex];
            }
        }
    }
}
