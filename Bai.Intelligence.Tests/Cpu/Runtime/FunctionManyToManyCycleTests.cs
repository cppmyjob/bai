using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Tests.Infrastructure;
using Bai.Intelligence.Tests.Infrastructure.Stubs;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cpu.Runtime
{
    public class FunctionManyToManyCycleTests
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
            var cycle = new FunctionManyToManyCycle(1);

            var function = new NeuronFunctionManyToManyStub();
            var item1 = new FunctionManyToManyCycle.Item {
                Function = function, 
                InputValueIndexes = new[] { 0, 1 },
                TempOutputIndexes = new[] { 2, 3 }
            };
            cycle.Items.Add(item1);

            // ACT
            var tempMemory = new float[] { 33, 44, 0, 0 };
            cycle.Compute(tempMemory);

            // ASSERT
            Assert.AreEqual(1, NeuronFunctionManyToManyStub.CallCount);
            Assert.AreEqual(new [] { 33F, 44F }, NeuronFunctionManyToManyStub.InputValues);
            Assert.AreEqual(55F, tempMemory[2]);
            Assert.AreEqual(66F, tempMemory[3]);

        }
    }
}
