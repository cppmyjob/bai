using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests
{
    public class SumCycleTests
    {
        private TestEnvBase _env;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        [Test]
        public void ShouldCompute()
        {
            // ARRANGE
            var cycle = new SumCycle(1);
            var item1 = new SumCycle.Item() { NeuronIndex = 0, ResultIndex = 3, Indexes = new []{0, 1, 2}};
            cycle.Items.Add(item1);

            // ACT
            var tempMemory = new float[] { 3, 5, 7, 0 };
            cycle.Compute(tempMemory);

            // ASSERT
            Assert.AreEqual(15F, tempMemory[3]);
        }
    }
}
