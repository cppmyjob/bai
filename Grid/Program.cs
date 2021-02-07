using System;
using ManagedCuda;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Grid tests!");

            int N = 2048;

            var deviceCount = CudaContext.GetDeviceCount();

            for (int i = 0; i < deviceCount; i++)
            {
                var properties = CudaContext.GetDeviceInfo(i);
                Console.WriteLine($"GPU {properties.DeviceName}");
                GpuGridTests(N, i);
            }

            Console.ReadKey();
        }

        public static void GpuGridTests(int N, int deviceID)
        {
            using CudaContext ctx = new CudaContext(deviceID);
            CudaKernel kernel = ctx.LoadKernel("kernel.ptx", "Grid");

            var blockDimensions = 256;

            kernel.GridDimensions = (N + 255) / blockDimensions;
            kernel.BlockDimensions = blockDimensions;

            var blockDimX = new CudaDeviceVariable<int>(N);
            var blockIdxX = new CudaDeviceVariable<int>(N);
            var threadIdxX = new CudaDeviceVariable<int>(N);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // Invoke kernel
            kernel.Run(blockDimX.DevicePointer, blockIdxX.DevicePointer, threadIdxX.DevicePointer, N);
            watch.Stop();
            Console.WriteLine($"GPU Time {watch.ElapsedMilliseconds} ms");

            // Copy result from device memory to host memory
            // h_C contains the result in host memory
            int[] blockDimXFrom = blockDimX;
            int[] blockIdxXFrom = blockIdxX;
            int[] threadIdxXFrom = threadIdxX;
        }

    }
}
