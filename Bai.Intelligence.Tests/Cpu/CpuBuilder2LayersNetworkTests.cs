using Bai.Intelligence.Cpu;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cpu
{
    public class CpuBuilder2LayersNetworkTests
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
            var definition = _env.Create2LayersNetwork();
            var inputOutputCount = definition.InputCount + 4;

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.AreEqual(inputOutputCount + 16, runtime.TempMemory.Length);
        }

        [Test]
        public void ShouldBeCorrectCycles()
        {
            // ARRANGE
            var definition = _env.Create2LayersNetwork();

            // ACT
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);

            // ASSERT
            Assert.AreEqual(6, runtime.Cycles.Count);
        }
    }
}
