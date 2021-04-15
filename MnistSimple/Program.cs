using Bai.Intelligence.DataSets;
using System;
using System.IO;
using System.Linq;
using Bai.Intelligence.Data;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Models.Optimizers;

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
        }
    }

}
