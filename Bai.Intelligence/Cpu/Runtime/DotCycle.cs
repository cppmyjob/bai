using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using Bai.Intelligence.Collections;
using MemoryPools;
using MemoryPools.Collections.Specialized;

namespace Bai.Intelligence.Cpu.Runtime
{
    public class DotCycle : Cycle
    {

        public class DotProduct
        {
            public int NeuronIndex;
            public int OutputIndex;
            public float[] Weights;

            public DotProduct Init(int neuronIndex, int outputIndex, float[] weights)
            {
                NeuronIndex = neuronIndex;
                OutputIndex = outputIndex;
                Weights = weights;
                return this;
            }
        }

        public class InputData : IDisposable
        {
            public int[] SourceIndexes;
            public PList<DotProduct> DotProducts;

            public InputData()
            {
                DotProducts = Pool<PList<DotProduct>>.Get().Init();
            }

            public void AddProduct(int neuronIndex, int outputIndex, float[] weights)
            {
                var product = Pool<DotProduct>.Get().Init(neuronIndex, outputIndex, weights);
                DotProducts.Add(product);
            }

            public void Dispose()
            {
                if (DotProducts != null)
                {
                    for (var i = 0; i < DotProducts.Count; i++)
                    {
                        Pool<DotProduct>.Return(DotProducts[i]);
                        DotProducts[i] = default;
                    }
                    DotProducts.Dispose();
                    Pool<PList<DotProduct>>.Return(DotProducts);
                    DotProducts = default;
                }
            }
        }

        public DotCycle(int[] sourceIndexes)
        {
            Inputs = new DotCycle.InputData
                     {
                         SourceIndexes = sourceIndexes, 
                     };
        }

        public InputData Inputs { get; private set; }


        public override void Compute(float[] tempMemory)
        {
            var inputCount = Inputs.SourceIndexes.Length;
            Span<float> inputData = stackalloc float[inputCount];

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
                    var va = new Vector<float>(inputData.Slice(j, vecSize));
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

        public override void Dispose()
        {
            Inputs?.Dispose();
            Inputs = default;
        }

        private void ComputeNative(float[] tempMemory)
        {
            var inputCount = Inputs.SourceIndexes.Length;
            Span<float> inputData = stackalloc float[inputCount];

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
