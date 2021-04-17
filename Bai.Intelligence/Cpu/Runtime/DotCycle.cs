using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class DotCycle : Cycle
    {

        public class DotProduct
        {
            public float[] Weights;
            public int OutputIndex;
            public int NeuronIndex;
        }

        public class InputData
        {
            public int[] SourceIndexes;
            public List<DotProduct> DotProducts;
        }

        public DotCycle(InputData inputs)
        {
            Inputs = inputs;
        }

        public InputData Inputs { get; }


        public override void Compute(float[] tempMemory)
        {
            var inputCount = Inputs.SourceIndexes.Length;
            var inputData = new float[inputCount];

            for (int i = 0; i < inputCount; i++)
            {
                inputData[i] = tempMemory[Inputs.SourceIndexes[i]];
            }

            var vecSize = Vector<float>.Count;

            if (inputData.Length < vecSize)
            {
                ComputeNative(tempMemory);
                return;
            }

            var vectorOperationsLength = inputData.Length / vecSize * vecSize;
            var nativeOperationIndex = inputData.Length % vecSize;
            if (nativeOperationIndex > 0)
                nativeOperationIndex = vectorOperationsLength;

            var inputsDotProductsCount = Inputs.DotProducts.Count;
            for (var i = 0; i < inputsDotProductsCount; i++)
            {
                var vsum = new Vector<float>(0.0f);

                var product = Inputs.DotProducts[i];
                var weights = product.Weights;
                for (var j = 0; j < vectorOperationsLength; j += vecSize)
                {
                    var va = new Vector<float>(inputData, j);
                    var vb = new Vector<float>(weights, j);
                    vsum += va * vb;
                }

                var sum = 0.0f;
                for (int j = 0; j < vecSize; j++)
                {
                    sum += vsum[j];
                }

                if (nativeOperationIndex > 0)
                {
                    for (int j = nativeOperationIndex; j < inputData.Length; j++)
                    {
                        sum += inputData[j] * product.Weights[j];
                    }
                }

                tempMemory[product.OutputIndex] = sum;
            }
        }

        private void ComputeNative(float[] tempMemory)
        {
            var inputCount = Inputs.SourceIndexes.Length;
            var inputData = new float[inputCount];

            for (int i = 0; i < inputCount; i++)
            {
                inputData[i] = tempMemory[Inputs.SourceIndexes[i]];
            }

            for (var i = 0; i < Inputs.DotProducts.Count; i++)
            {
                var sum = 0.0f;
                var product = Inputs.DotProducts[i];
                for (var j = 0; j < inputData.Length; j++)
                {
                    sum += inputData[j] * product.Weights[j];
                }

                tempMemory[product.OutputIndex] = sum;
            }

        }


        //public override void Compute(float[] tempMemory)
        //{
        //    var inputCount = Inputs.SourceIndexes.Count;
        //    var inputData = new float[inputCount];

        //    for (int i = 0; i < inputCount; i++)
        //    {
        //        inputData[i] = tempMemory[Inputs.SourceIndexes[i]];
        //    }

        //    var vecSize = Vector<float>.Count;

        //    for (var i = 0; i < Inputs.DotProducts.Count; i++)
        //    {
        //        var sum = 0.0f;

        //        var product = Inputs.DotProducts[i];
        //        var weights = product.Weights;
        //        for (var j = 0; j < inputData.Length; j += vecSize)
        //        {
        //            var va = new Vector<float>(inputData, j);
        //            var vb = new Vector<float>(weights, j);
        //            sum += Vector.Dot(va,vb);
        //        }

        //        tempMemory[product.OutputIndex] = sum;
        //    }

        //}


        //public override void Compute(float[] tempMemory)
        //{
        //    var inputCount = Inputs.SourceIndexes.Count;
        //    var inputData = new float[inputCount];

        //    for (int i = 0; i < inputCount; i++)
        //    {
        //        inputData[i] = tempMemory[Inputs.SourceIndexes[i]];
        //    }

        //    for (var i = 0; i < Inputs.DotProducts.Count; i++)
        //    {
        //        var sum = 0.0f;
        //        var product = Inputs.DotProducts[i];
        //        for (var j = 0; j < inputData.Length; j++)
        //        {
        //            sum += inputData[j] * product.Weights[j];
        //        }

        //        tempMemory[product.OutputIndex] = sum;
        //    }

        //}
    }
}
