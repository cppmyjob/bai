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
            Assert.AreEqual(inputOutputCount + 1, runtime.TempMemory.Length);
        }

        [Test]
        public void ShouldHaveValidCyclesCount()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();
            var inputOutputCount = definition.InputCount + definition.OutputCount;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.AreEqual(runtime.Cycles.Count, 2);
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
            Assert.IsInstanceOf(typeof(DotCycle), runtime.Cycles[0]);
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
            var cycle = (DotCycle) runtime.Cycles[0];
            Assert.AreEqual(new [] {0, 1, 2}, cycle.Inputs.SourceIndexes);

            Assert.AreEqual(1, cycle.Inputs.DotProducts.Count);
            var dotProduct = cycle.Inputs.DotProducts[0];

            Assert.AreEqual(new[] {2.2F, 3.3F, 1.1F}, dotProduct.Weights);
            Assert.AreEqual(0, dotProduct.NeuronIndex);
            Assert.AreEqual(4, dotProduct.OutputIndex);
        }

        [Test]
        public void FunctionCycleShouldBeSecond()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.GreaterOrEqual(runtime.Cycles.Count, 2);
            Assert.IsInstanceOf(typeof(FunctionOneToOneCycle), runtime.Cycles[1]);
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
            var cycle = (FunctionOneToOneCycle)runtime.Cycles[1];

            Assert.AreEqual(1, cycle.Items.Count);
            var item = cycle.Items[0];

            Assert.AreEqual(inputOutputCount, item.InputValueIndex);
            Assert.AreEqual(3, item.TempOutputIndex);

            Assert.IsInstanceOf(typeof(SigmoidFunction), item.Function);
            var function = (SigmoidFunction) item.Function;
            Assert.AreEqual(0.1, function.Alfa, 0.00001);
        }
    }
}
