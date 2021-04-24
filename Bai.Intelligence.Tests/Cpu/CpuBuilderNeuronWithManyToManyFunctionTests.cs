using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Organism.Functions;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cpu
{
    public class CpuBuilderNeuronWithManyToManyFunctionTests
    {
        private TestEnvBase _env;
        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        [Test]
        public void ShouldAllocateValidMemorySize()
        {
            // ARRANGE
            var definition = _env.CreateNeuronWithManyToManyFunction();
            var inputOutputCount = definition.InputCount + definition.OutputCount;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.AreEqual(inputOutputCount + 3, runtime.TempMemoryLength);
        }


        [Test]
        public void MultiCycleShouldBeFirst()
        {
            // ARRANGE
            var definition = _env.CreateNeuronWithManyToManyFunction();

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.GreaterOrEqual(runtime.Cycles.Count, 1);
            Assert.IsInstanceOf(typeof(MultiCycle), runtime.Cycles[0]);
        }

        [Test]
        public void MultiCycleShouldHaveCorrectValues()
        {
            // ARRANGE
            var definition = _env.CreateNeuronWithManyToManyFunction();
            var inputOutputCount = definition.InputCount + definition.OutputCount;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            var multiCycle = (MultiCycle)runtime.Cycles[0];
            Assert.AreEqual(3, multiCycle.Items.Count);

            var item1 = multiCycle.Items[0];
            Assert.AreEqual(inputOutputCount + 0, item1.OutputIndex);
            Assert.AreEqual(0, item1.SourceIndex);
            Assert.AreEqual(2.2, item1.Weight, 0.0001);

            var item2 = multiCycle.Items[1];
            Assert.AreEqual(inputOutputCount + 1, item2.OutputIndex);
            Assert.AreEqual(1, item2.SourceIndex);
            Assert.AreEqual(3.3, item2.Weight, 0.0001);

            var item3 = multiCycle.Items[2];
            Assert.AreEqual(inputOutputCount + 2, item3.OutputIndex);
            Assert.AreEqual(2, item3.SourceIndex);
            Assert.AreEqual(1.1, item3.Weight, 0.0001);
        }


        [Test]
        public void FunctionCycleShouldBeSecond()
        {
            // ARRANGE
            var definition = _env.CreateNeuronWithManyToManyFunction();

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.GreaterOrEqual(runtime.Cycles.Count, 2);
            Assert.IsInstanceOf(typeof(FunctionManyToManyCycle), runtime.Cycles[1]);
        }


        [Test]
        public void FunctionCycleShouldHaveCorrectValues()
        {
            // ARRANGE
            var definition = _env.CreateNeuronWithManyToManyFunction();
            var inputOutputCount = definition.InputCount + definition.OutputCount;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            var cycle = (FunctionManyToManyCycle)runtime.Cycles[1];

            Assert.AreEqual(1, cycle.Items.Count);
            var item = cycle.Items[0];

            Assert.AreEqual(new [] { 6, 7, 8 }, item.InputValueIndexes);
            Assert.AreEqual(new [] { 3, 4, 5 }, item.TempOutputIndexes);

            Assert.IsInstanceOf(typeof(SoftMaxFunction), item.Function);
        }
    }
}
