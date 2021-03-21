using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Definition;

namespace Bai.Intelligence.Genetic
{
    public class GeneticInitData
    {
        public GeneticInitData(int itemsNumber, int surviveNumber, int repeatNumber, int processorsNumber)
        {
            ItemsNumber = itemsNumber;
            SurviveNumber = surviveNumber;
            RepeatNumber = repeatNumber;
            ProcessorsNumber = processorsNumber;

            if (SurviveNumber > ItemsNumber)
                throw new ArgumentException("Survive Number > Person Number");
            if (SurviveNumber < 1)
                throw new ArgumentException("Survive Numbe < 1");
            if (ItemsNumber < 2)
                throw new ArgumentException("Person Number < 2");
        }

        public int ItemsNumber { get; }
        public int SurviveNumber { get; }
        public int RepeatNumber { get; }
        public int ProcessorsNumber { get; }

        public int InputCount { get; set; }
        public int OutputCount { get; set; }

        public NetworkDefinition[] Men { get; set; }
        public NetworkDefinition[] Women { get; set; }
    }
}
