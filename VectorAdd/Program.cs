using System;
using ManagedCuda;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //int N = 50000000;
            int N = 50000000 * 2;

            var deviceCount = CudaContext.GetDeviceCount();

            for (int i = 0; i < deviceCount; i++)
            {
                var properties = CudaContext.GetDeviceInfo(i);
                Console.WriteLine($"GPU {properties.DeviceName}");
                GpuVectorAdd(N, i);
            }

            CpuVectorAdd(N);

            Console.ReadKey();
        }

        public static void CpuVectorAdd(int N)
        {
            // Allocate input vectors h_A and h_B in host memory
            var (h_A, h_B) = InitArrays(N);

            // Invoke kernel
            float[] h_C = new float[N];
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < N; i++)
            {
                h_C[i] = h_A[i] + h_B[i] + 5;
            }
            watch.Stop();
            Console.WriteLine($"CPU Time {watch.ElapsedMilliseconds} ms");
        }

        public static void GpuVectorAdd(int N, int deviceID)
        {
            using CudaContext ctx = new CudaContext(deviceID);
            CudaKernel kernel = ctx.LoadKernel("kernel.ptx", "VecAdd");

            var blockDimensions = 256;

            kernel.GridDimensions = (N + 255) / blockDimensions;
            kernel.BlockDimensions = blockDimensions;

            // Allocate input vectors h_A and h_B in host memory
            var (h_A, h_B) = InitArrays(N);

            // Allocate vectors in device memory and copy vectors from host memory to device memory 

            CudaDeviceVariable<float> d_A = h_A;
            CudaDeviceVariable<float> d_B = h_B;
            CudaDeviceVariable<float> d_C = new CudaDeviceVariable<float>(N);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // Invoke kernel
            kernel.Run(d_A.DevicePointer, d_B.DevicePointer, d_C.DevicePointer, N);
            watch.Stop();
            Console.WriteLine($"GPU Time {watch.ElapsedMilliseconds} ms");

            // Copy result from device memory to host memory
            // h_C contains the result in host memory
            float[] h_C = d_C;
        }

        private static (float[] h_A, float[] h_B) InitArrays(int N)
        {
            float[] h_A = new float[N];
            float[] h_B = new float[N];
            for (int i = 0; i < N; i++)
            {
                h_A[i] = 1;
                h_B[i] = 5;
            }

            return (h_A, h_B);
        }
    }
}
