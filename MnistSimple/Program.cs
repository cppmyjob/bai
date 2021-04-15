using Bai.Intelligence.DataSets;
using System;
using System.IO;
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
            var trainX = new DataArray(768);
            trainX.AddRange(data.Train.X);
            trainX = trainX / 255;

            var trainY = new DataArray(1);
            trainY.AddRange(data.Train.Y);
            trainY = trainY.ToCategorical(10);

            var model = new Sequential();
            model.Layers.Add(new Dense(800, inputDim: 768, activation: ActivationType.Sigmoid));
            model.Layers.Add(new Dense(10, activation: ActivationType.Softmax));

            var optimizer = new GeneticOptimizer();
            model.Compile(optimizer);

            model.Fit(trainX, trainY);
        }
    }

}
