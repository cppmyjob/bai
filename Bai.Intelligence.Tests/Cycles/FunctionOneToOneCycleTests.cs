using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Tests.Infrastructure;
using Bai.Intelligence.Tests.Infrastructure.Stubs;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cycles
{
    public class FunctionOneToOneCycleTests
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
            var cycle = new FunctionOneToOneCycle(1);

            var function = new NeuronFunctionOneToOneStub();
            var item1 = new FunctionOneToOneCycle.Item() {Function = function, TempOutputIndex = 1, InputValueIndex = 0};
            cycle.Items.Add(item1);

            // ACT
            var tempMemory = new float[] { 33, 0 };
            cycle.Compute(tempMemory);

            // ASSERT
            Assert.AreEqual(1, NeuronFunctionOneToOneStub.CallCount);
            Assert.AreEqual(33F, NeuronFunctionOneToOneStub.InputValue);
            Assert.AreEqual(55F, tempMemory[1]);

        }
    }
}
