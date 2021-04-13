using Bai.Intelligence.DataSets;
using System;
using System.IO;
using Bai.Intelligence.Data;

namespace MnistSimple
{

    //public class DigitImage
    //{
    //    public byte[,] pixels;
    //    public byte label;

    //    public DigitImage(byte[,] pixels,
    //        byte label)
    //    {
    //        this.pixels = new byte[28, 28];

    //        for (int i = 0; i < 28; ++i)
    //        for (int j = 0; j < 28; ++j)
    //            this.pixels[i,j] = pixels[i,j];

    //        this.label = label;
    //    }

    //    public override string ToString()
    //    {
    //        string s = "";
    //        for (int i = 0; i < 28; ++i)
    //        {
    //            for (int j = 0; j < 28; ++j)
    //            {
    //                if (this.pixels[i,j] == 0)
    //                    s += " "; // white
    //                else if (this.pixels[i,j] == 255)
    //                    s += "O"; // black
    //                else
    //                    s += "."; // gray
    //            }
    //            s += "\n";
    //        }
    //        s += this.label.ToString();
    //        return s;
    //    } // ToString

    //}

    class Program
    {
        static void Main(string[] args)
        {
            var data = FashionMnist.LoadData();
            var train = new DataArray(768);
            train.AddRange(data.Train.X);
            train = train / 255;

        }
    }

    //FashionMnist

    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        try
    //        {
    //            Console.WriteLine("\nBegin\n");
    //            FileStream ifsLabels =
    //                new FileStream(@"train-labels-idx1-ubyte",
    //                    FileMode.Open); // test labels
    //            FileStream ifsImages =
    //                new FileStream(@"train-images-idx3-ubyte",
    //                    FileMode.Open); // test images

    //            BinaryReader brLabels =
    //                new BinaryReader(ifsLabels);
    //            BinaryReader brImages =
    //                new BinaryReader(ifsImages);

    //            var magic1 = ReverseBytes(brImages.ReadInt32()); // discard
    //            var numImages = ReverseBytes(brImages.ReadInt32());
    //            var numRows = ReverseBytes(brImages.ReadInt32());
    //            var numCols = ReverseBytes(brImages.ReadInt32());

    //            int magic2 = ReverseBytes(brLabels.ReadInt32());
    //            int numLabels = ReverseBytes(brLabels.ReadInt32());


    //            byte[,] pixels = new byte[28, 28];

    //            // each test image
    //            for (int di = 0; di < 10000; ++di)
    //            {
    //                for (int i = 0; i < 28; ++i)
    //                {
    //                    for (int j = 0; j < 28; ++j)
    //                    {
    //                        byte b = brImages.ReadByte();
    //                        pixels[i,j] = b;
    //                    }
    //                }

    //                byte lbl = brLabels.ReadByte();

    //                DigitImage dImage =
    //                    new DigitImage(pixels, lbl);
    //                Console.WriteLine(dImage.ToString());
    //                Console.ReadLine();
    //            } // each image

    //            ifsImages.Close();
    //            brImages.Close();
    //            ifsLabels.Close();
    //            brLabels.Close();

    //            Console.WriteLine("\nEnd\n");
    //            Console.ReadLine();
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            Console.ReadLine();
    //        }
    //    }

    //    public static int ReverseBytes(int v)
    //    {
    //        byte[] intAsBytes = BitConverter.GetBytes(v);
    //        Array.Reverse(intAsBytes);
    //        return BitConverter.ToInt32(intAsBytes, 0);
    //    }
    //}
}
