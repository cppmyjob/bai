using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bai.Intelligence.Collections;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Tests.Infrastructure;
using MemoryPools.Collections.Linq;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cpu.Runtime
{
    public class DotCycleTests
    {
        private TestEnvBase _env;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        [Test]
        public void SimpleTest()
        {
            // ARRANGE

            var cycle = new DotCycle(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10});
            cycle.Inputs.DotProducts.Add(
                        new DotCycle.DotProduct
                        {
                            Weights = new[] { 0.1F, 0.2F, 0.3F, 0.4F, 0.5F, 0.6F, 0.7F, 0.8F, 0.9F, 1.0F, 1.1F },
                            OutputIndex = 11
                        }
            );


            // ACT
            var tempArray = new float[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0};
            cycle.Compute(tempArray);

            // ASSERT

            Assert.AreEqual(50.6F, tempArray[11], 0.0001);

        }

        [Test]
        public void PerformanceTest()
        {
            List<DotCycle.DotProduct> CreateDotProducts(int inputCount, int neuronCount)
            {
                return Enumerable.Range(0, neuronCount).Select(ni =>
                       
                           new DotCycle.DotProduct
                           {
                               Weights = Enumerable.Range(0, inputCount).Select(t => (float)t / 10).ToArray(),
                               OutputIndex = ni + inputCount
                           }
                       ).ToList();
            }
            // ARRANGE

            var cycle1InputCount = 28 * 28;
            var cycle1NeuronCount = 800;
            var cycle1 = new DotCycle(Enumerable.Range(0, cycle1InputCount).ToArray());
            var list1 = CreateDotProducts(cycle1InputCount, cycle1NeuronCount);
            for (var i = 0; i < list1.Count; i++)
            {
                cycle1.Inputs.DotProducts.Add(list1[i]);
            }

            var cycle2InputCount = 800;
            var cycle2NeuronCount = 10;
            var cycle2 = new DotCycle(Enumerable.Range(0, cycle2InputCount).ToArray());
            var list2 = CreateDotProducts(cycle2InputCount, cycle2NeuronCount);
            for (var i = 0; i < list2.Count; i++)
            {
                cycle2.Inputs.DotProducts.Add(list2[i]);
            }

            // ACT
            var tempArray = new float[10000];

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < 10000; i++)
            {
                cycle1.Compute(tempArray);
                cycle2.Compute(tempArray);
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine("Calculate RunTime " + elapsedTime);
            // ASSERT

            Assert.True(true);

        }

    }
}
