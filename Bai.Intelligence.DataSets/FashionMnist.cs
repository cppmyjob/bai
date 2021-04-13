using Bai.Intelligence.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Bai.Intelligence.DataSets
{
    public static class FashionMnist
    {
        public static string DataFolder = "data";

        public static string TestImagesName = "t10k-images-idx3-ubyte";
        public static string TestLabelsName = "t10k-labels-idx1-ubyte";

        public static string TrainImagesName = "train-images-idx3-ubyte";
        public static string TrainLabelsName = "train-labels-idx1-ubyte";

        public static (DataSet<List<byte[,]>, List<byte>> Train, DataSet<List<byte[,]>, List<byte>> Test) LoadData()
        {
            UnzipFile(TestImagesName);
            UnzipFile(TestLabelsName);

            UnzipFile(TrainImagesName);
            UnzipFile(TrainLabelsName);

            var testImages = LoadImages(TestImagesName);
            var testLabels = LoadLabels(TestLabelsName);

            var trainImages = LoadImages(TrainImagesName);
            var trainLabels = LoadLabels(TrainLabelsName);

            var test = new DataSet<List<byte[,]>, List<byte>>(testImages, testLabels);
            var train = new DataSet<List<byte[,]>, List<byte>>(trainImages, trainLabels);

            return (train, test);
        }

        private static List<byte> LoadLabels(string filename)
        {
            var result = new List<byte>();
            var fileNameInFolder = $"{DataFolder}/{filename}";
            using (var ifsImages = new FileStream(fileNameInFolder, FileMode.Open))
            {
                using (var brLabels = new BinaryReader(ifsImages))
                {
                    var magic2 = ReverseBytes(brLabels.ReadInt32());
                    var numLabels = ReverseBytes(brLabels.ReadInt32());

                    for (int i = 0; i < numLabels; i++)
                    {
                        byte lbl = brLabels.ReadByte();
                        result.Add(lbl);
                    }
                }
            }
            return result;
        }


        private static List<byte[,]> LoadImages(string filename)
        {
            var result = new List<byte[,]>();
            var fileNameInFolder = $"{DataFolder}/{filename}";
            using (var ifsImages = new FileStream(fileNameInFolder, FileMode.Open))
            {
                using (var brImages = new BinaryReader(ifsImages))
                {
                    var magic1 = ReverseBytes(brImages.ReadInt32());
                    var numImages = ReverseBytes(brImages.ReadInt32());
                    var numRows = ReverseBytes(brImages.ReadInt32());
                    var numCols = ReverseBytes(brImages.ReadInt32());
                    
                    for (int di = 0; di < numImages; ++di)
                    {
                        byte[,] pixels = new byte[numCols, numRows];

                        for (int i = 0; i < numRows; ++i)
                        {
                            for (int j = 0; j < numCols; ++j)
                            {
                                byte b = brImages.ReadByte();
                                pixels[i, j] = b;
                            }
                        }
                        result.Add(pixels);
                    }
                }
            }
            return result;
        }

        public static int ReverseBytes(int v)
        {
            byte[] intAsBytes = BitConverter.GetBytes(v);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }

        private static void UnzipFile(string filename)
        {
            var fileNameInFolder = $"{DataFolder}/{filename}";
            if (File.Exists(fileNameInFolder))
            {
                return;
            }

            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }

            var zipFile = $"{DataFolder}/{filename}.zip";
            if (!File.Exists(zipFile))
            {
                var type = typeof(FashionMnist);
                var assembly = type.Assembly;
                using (var resourceStream = assembly.GetManifestResourceStream($"{type.Namespace}.Mnist.{filename}.zip"))
                using (var file = File.Create(zipFile))
                {
                    resourceStream.CopyTo(file);
                }
            }
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFile, DataFolder);
        }
    }
}
