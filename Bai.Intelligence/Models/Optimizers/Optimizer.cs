using Bai.Intelligence.Organism.Definition;

namespace Bai.Intelligence.Models.Optimizers
{
    public abstract class Optimizer
    {
        public abstract void Run(NetworkDefinition networkDefinition);
    }
}
