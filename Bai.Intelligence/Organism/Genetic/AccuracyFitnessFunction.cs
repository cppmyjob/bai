using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Data;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Organism.Definition;

namespace Bai.Intelligence.Organism.Genetic
{
    public class AccuracyFitnessFunction : IFitnessFunction<NetworkDefinition>
    {
        public double Calculate(InputDataArray trainX, InputDataArray trainY, NetworkDefinition item)
        {
            var builder = new CpuBuilder();
            var runtime = builder.Build(item);
            runtime.SetInputMemory(trainX.Data);


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var inputData = new[] {new InputData()};
            int i, j, k;
            var meanSum = 0;
            for (i = 0, j = 0, k = 0; i < trainX.Data.Length; i += trainX.FrameLength, j += trainY.FrameLength, ++k)
            {
                inputData[0].Length = trainX.FrameLength;
                inputData[0].Offset = i;
                var result = runtime.Compute(inputData);
                var predictIndex = GetMaxIndex(result, 0, result.Length);
                var expectedIndex = GetMaxIndex(trainY.Data, j, trainY.FrameLength);
                meanSum += predictIndex == expectedIndex ? 1 : 0;
                //Console.WriteLine($"Calculate = {k}");
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("Calculate RunTime " + elapsedTime);

            var allCount = trainX.Data.Length / trainX.FrameLength;
            return (float)meanSum / allCount;
        }

        private int GetMaxIndex(float[] values, int offset, int length)
        {
            var max = float.MinValue;
            var maxIndex = -1; 
            for (var i = 0; i < length; i++)
            {
                var value = values[offset + i];
                if (!(value > max)) continue;
                max = value;
                maxIndex = i;
            }
            return maxIndex;
        }
    }
}

