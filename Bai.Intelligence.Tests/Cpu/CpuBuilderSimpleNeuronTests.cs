using Bai.Intelligence.Cpu;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Organism.Functions;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cpu
{
    public class CpuBuilderSimpleNeuronTests
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
            var definition = _env.CreateSimpleNeuron();
            var inputOutputCount = definition.InputCount + definition.OutputCount;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.AreEqual(inputOutputCount + 4, runtime.TempMemory.Length);
        }

        [Test]
        public void MultiCycleShouldBeFirst()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.GreaterOrEqual(runtime.Cycles.Count,  1);
            Assert.IsInstanceOf(typeof(MultiCycle), runtime.Cycles[0]);
        }

        [Test]
        public void MultiCycleShouldHaveCorrectValues()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();
            var inputOutputCount = definition.InputCount + definition.OutputCount;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            var multiCycle = (MultiCycle) runtime.Cycles[0];
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
        public void SumCycleShouldBeSecond()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.GreaterOrEqual(runtime.Cycles.Count, 2);
            Assert.IsInstanceOf(typeof(SumCycle), runtime.Cycles[1]);
        }

        [Test]
        public void SumCycleShouldHaveCorrectValues()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();
            var inputOutputCount = definition.InputCount + definition.OutputCount;
            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            var cycle = (SumCycle)runtime.Cycles[1];

            Assert.AreEqual(1, cycle.Items.Count);
            var item = cycle.Items[0];

            Assert.AreEqual(3, item.Indexes.Length);
            Assert.AreEqual(
                new [] {inputOutputCount + 0, inputOutputCount + 1, inputOutputCount + 2 }, 
                item.Indexes);
            Assert.AreEqual(0, item.NeuronIndex);
            Assert.AreEqual(inputOutputCount + 3, item.ResultIndex);
        }

        [Test]
        public void FunctionCycleShouldBeThird()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.GreaterOrEqual(runtime.Cycles.Count, 3);
            Assert.IsInstanceOf(typeof(FunctionOneToOneCycle), runtime.Cycles[2]);
        }


        [Test]
        public void FunctionCycleShouldHaveCorrectValues()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();
            var inputOutputCount = definition.InputCount + definition.OutputCount;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            var cycle = (FunctionOneToOneCycle)runtime.Cycles[2];

            Assert.AreEqual(1, cycle.Items.Count);
            var item = cycle.Items[0];

            Assert.AreEqual(inputOutputCount + 3, item.InputValueIndex);
            Assert.AreEqual(3, item.TempOutputIndex);

            Assert.IsInstanceOf(typeof(SigmoidFunction), item.Function);
            var function = (SigmoidFunction) item.Function;
            Assert.AreEqual(0.1, function.Alfa, 0.00001);
        }
    }
}
