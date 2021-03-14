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
    public class MultiCycleTests
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
            var cycle = new MultiCycle(1);
            var item1 = new MultiCycle.Item { NeuronIndex = 0, OutputIndex = 0, SourceIndex = 0, Weight = 10 };
            var item2 = new MultiCycle.Item { NeuronIndex = 0, OutputIndex = 1, SourceIndex = 1, Weight = 20 };
            var item3 = new MultiCycle.Item { NeuronIndex = 0, OutputIndex = 2, SourceIndex = 2, Weight = 30 };
            cycle.Items.Add(item1);
            cycle.Items.Add(item2);
            cycle.Items.Add(item3);

            // ACT
            var memory = new float[] {3, 4, 5};
            var tempMemory = new float[3];
            cycle.Compute(memory, tempMemory);

            // ASSERT
            Assert.AreEqual(10F * 3, tempMemory[0]);
            Assert.AreEqual(20F * 4, tempMemory[1]);
            Assert.AreEqual(30F * 5, tempMemory[2]);
        }

    }
}
