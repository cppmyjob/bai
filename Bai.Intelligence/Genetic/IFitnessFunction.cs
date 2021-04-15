using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Data;

namespace Bai.Intelligence.Genetic
{
    public interface IFitnessFunction<TGeneticItem>
    {
        double Calculate(InputDataArray trainX, InputDataArray trainY, TGeneticItem item);
    }
}
