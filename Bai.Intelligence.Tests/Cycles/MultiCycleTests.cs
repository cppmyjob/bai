using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cycles
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
            var item1 = new MultiCycle.Item { NeuronIndex = 0, OutputIndex = 3, SourceIndex = 0, Weight = 10 };
            var item2 = new MultiCycle.Item { NeuronIndex = 0, OutputIndex = 4, SourceIndex = 1, Weight = 20 };
            var item3 = new MultiCycle.Item { NeuronIndex = 0, OutputIndex = 5, SourceIndex = 2, Weight = 30 };
            cycle.Items.Add(item1);
            cycle.Items.Add(item2);
            cycle.Items.Add(item3);

            // ACT
            var tempMemory = new float[] { 3, 4, 5, -1, -1, -1 };
            cycle.Compute(tempMemory);

            // ASSERT
            Assert.AreEqual(10F * 3, tempMemory[3]);
            Assert.AreEqual(20F * 4, tempMemory[4]);
            Assert.AreEqual(30F * 5, tempMemory[5]);
        }

    }
}
