using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Data;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Genetic
{
    public interface IFitnessFunction<TGeneticItem>
    {
        double Calculate(ILogger logger, InputDataArray trainX, InputDataArray trainY, TGeneticItem item);
    }
}
