using Bai.Intelligence.Data;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism.Definition;

namespace Bai.Intelligence.Models.Optimizers
{
    public abstract class Optimizer
    {
        public abstract void Run(ILogger logger, NetworkDefinition networkDefinition,
            DataArray x, DataArray y);
    }
}
