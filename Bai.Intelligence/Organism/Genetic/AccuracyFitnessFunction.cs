using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Data;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Utils;

namespace Bai.Intelligence.Organism.Genetic
{
    public class AccuracyFitnessFunction : IFitnessFunction<NetworkDefinition>
    {
        public double Calculate(ILogger logger, InputDataArray trainX, InputDataArray trainY, NetworkDefinition item)
        {
            var builder = new CpuBuilder();

            var timeMeter1 = new TimeMeter(logger, "Build");
            timeMeter1.Start();
            using var runtime = builder.Build(item);
            timeMeter1.Stop();

            var timeMeter2 = new TimeMeter(logger, "Calculate");
            timeMeter2.Start();

            var inputData = new[] {new InputData()};
            int i, j;
            var meanSum = 0;
            for (i = 0, j = 0; i < trainX.Data.Length; i += trainX.FrameLength, j += trainY.FrameLength)
            {
                inputData[0].Length = trainX.FrameLength;
                inputData[0].Offset = i;
                var runtimeResult = runtime.Compute(trainX.Data, inputData);
                var predictIndex = GetMaxIndex(runtimeResult, 0, runtimeResult.Length);
                var expectedIndex = GetMaxIndex(trainY.Data, j, trainY.FrameLength);
                meanSum += predictIndex == expectedIndex ? 1 : 0;
            }

            var allCount = trainX.Data.Length / trainX.FrameLength;
            var result = (float)meanSum / allCount;
            timeMeter2.Stop($"Fitness:{result}");

            return result;
        }

        private int GetMaxIndex(ReadOnlySpan<float> values, int offset, int length)
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

