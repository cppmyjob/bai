using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Data;
using Bai.Intelligence.DataSets;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Models.Optimizers;
using Bai.Intelligence.Organism.Definition;

namespace MnistSimple
{
    public static class ImprovePerformance
    {

        public static void Execute()
        {
            var network = CreateNetwork();


            var builder = new CpuBuilder();
            var runtime = builder.Build(network.Network);


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var inputData = new[] { new InputData() };
            int i, j, k;
            var meanSum = 0;
            for (i = 0, j = 0, k = 0; i < network.TrainX.Data.Length; i += network.TrainX.FrameLength, j += network.TrainY.FrameLength, ++k)
            {
                inputData[0].Length = network.TrainX.FrameLength;
                inputData[0].Offset = i;
                var result = runtime.Compute(network.TrainX.Data, inputData);
                //var predictIndex = GetMaxIndex(result, 0, result.Length);
                //var expectedIndex = GetMaxIndex(trainY.Data, j, trainY.FrameLength);
                //meanSum += predictIndex == expectedIndex ? 1 : 0;
                //Console.WriteLine($"Calculate = {k}");
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("Calculate RunTime " + elapsedTime);

        }

        private static (NetworkDefinition Network, InputDataArray TrainX, InputDataArray TrainY) CreateNetwork()
        {
            var data = FashionMnist.LoadData();
            var trainX = new DataArray(784);

            var inputExamples = 10000;

            var x = data.Train.X.Take(inputExamples);
            trainX.AddRange(x);
            trainX = trainX / 255;

            var trainY = new DataArray(1);
            var y = data.Train.Y.Take(inputExamples);
            trainY.AddRange(y);
            trainY = trainY.ToCategorical(10);

            var model = new Sequential();
            model.Layers.Add(new Dense(800, inputDim: 784, activation: ActivationType.Sigmoid));
            model.Layers.Add(new Dense(10, activation: ActivationType.Softmax));

            var optimizer = new GeneticOptimizer();
            model.Compile(optimizer);

            var inputTrainX = new InputDataArray(trainX);
            var inputTrainY = new InputDataArray(trainY);

            return (model.NetworkDefinition, inputTrainX, inputTrainY);
        }

    }
}
