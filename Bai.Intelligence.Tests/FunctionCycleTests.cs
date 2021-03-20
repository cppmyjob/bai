using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Tests.Infrastructure;
using Bai.Intelligence.Tests.Infrastructure.Stubs;
using NUnit.Framework;

namespace Bai.Intelligence.Tests
{
    public class FunctionCycleTests
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
            var cycle = new FunctionCycle(1);

            var function = new NeuronFunctionStub();
            var item1 = new FunctionCycle.Item() {Function = function, TempOutputIndex = 1, InputValueIndex = 0};
            cycle.Items.Add(item1);

            // ACT
            var tempMemory = new float[] { 33, 0 };
            cycle.Compute(tempMemory);

            // ASSERT
            Assert.AreEqual(1, NeuronFunctionStub.CallCount);
            Assert.AreEqual(33F, NeuronFunctionStub.InputValue);
            Assert.AreEqual(55F, tempMemory[1]);

        }
    }
}
