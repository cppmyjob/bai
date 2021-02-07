#include <cuda_runtime.h>

extern "C"
__global__ void Grid(int* blockDimX, int* blockIdxX, int* threadIdxX)
{
    int i = blockDim.x * blockIdx.x + threadIdx.x;
    blockDimX[i] = blockDim.x;
    blockIdxX[i] = blockIdx.x;
    threadIdxX[i] = threadIdx.x;
}