using Bai.Intelligence.DataSets;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Bai.Intelligence.Data;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Models.Optimizers;
using Bai.Intelligence.Utils.Random;

namespace MnistSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = FashionMnist.LoadData();
            var trainX = new DataArray(784);

            var x = data.Train.X.Take(1000);
            trainX.AddRange(x);
            trainX = trainX / 255;

            var trainY = new DataArray(1);
            var y = data.Train.Y.Take(1000);
            trainY.AddRange(y);
            trainY = trainY.ToCategorical(10);

            var model = new Sequential();
            model.Layers.Add(new Dense(800, inputDim: 784, activation: ActivationType.Sigmoid));
            model.Layers.Add(new Dense(10, activation: ActivationType.Softmax));

            var optimizer = new GeneticOptimizer();
            model.Compile(optimizer);

            model.Fit(trainX, trainY);

            //NativeTest();
            //VectorTest();
        }

        private static void VectorTest()
        {
            int vecSize = Vector<float>.Count;
            var random = new Random();

            var arrayLength = 635000000;
            var a = new float[arrayLength];
            var b = new float[arrayLength];
            var c = new float[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                //a[i] = (float)random.NextDouble();
                //b[i] = (float)random.NextDouble();
                a[i] = 0;
                b[i] = 0;
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < arrayLength; i += vecSize)
            {
                var va = new Vector<float>(a, i);
                var vb = new Vector<float>(b, i);
                var vc = va * vb;
                vc.CopyTo(c, i);
            }

            //var va = new Vector<float>(a);
            //var vb = new Vector<float>(b);
            //var vc = va * vb;
            //vc.CopyTo(c);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine("Calculate RunTime " + elapsedTime);
        }

        private static void NativeTest()
        {

            //using var random = RandomFactory.Instance.Create();

            var arrayLength = 635000000;
            var a = new float[arrayLength];
            var b = new float[arrayLength];
            var c = new float[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                //a[i] = (float)random.NextDouble();
                //b[i] = (float)random.NextDouble();
                a[i] = 0;
                b[i] = 0;
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < arrayLength; i++)
            {
                c[i] = a[i] * b[i];
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine("Calculate RunTime " + elapsedTime);
        }
    }

}
