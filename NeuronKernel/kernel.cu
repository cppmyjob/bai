#include <cuda_runtime.h>

extern "C"
__global__ void Grid(const float* input, float* weights, int N)
{
    int i = blockDim.x * blockIdx.x + threadIdx.x;
    blockDimX[i] = blockDim.x;
    blockIdxX[i] = blockIdx.x;
    threadIdxX[i] = threadIdx.x;
}