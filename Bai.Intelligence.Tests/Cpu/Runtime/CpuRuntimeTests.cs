using System.Linq;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Data;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cpu.Runtime
{
    public class CpuRuntimeTests
    {
        private TestEnvBase _env;
        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        [Test]
        public void ShouldCalculateSimpleNeuron()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);
            var memory = new[] { 1F, 2F, 3F };

            // ACT
            var input = new InputData
            {
                Offset = 0,
                Length = memory.Length
            };
            var result = runtime.Compute(memory, new[] { input });

            // ASSERT
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0.770298949F, result[0]);
        }


        [Test]
        public void ShouldCalculate2LayersNetwork()
        {
            // ARRANGE
            var definition = _env.Create2LayersNetwork();
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);
            var memory = new[] { 1F, 2F, 3F };

            // ACT
            var input = new InputData
                        {
                            Offset = 0,
                            Length = memory.Length
                        };
            var result = runtime.Compute(memory, new[] { input });

            // ASSERT
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(0.8934361678F, result[0]);
        }


        [Test]
        public void ShouldCalculateNeuronWithManyToManyFunction()
        {
            // ARRANGE
            var definition = _env.CreateNeuronWithManyToManyFunction();
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);
            var memory = new[] { 1F, 2F, 3F };

            // ACT
            var input = new InputData
                        {
                            Offset = 0,
                            Length = memory.Length
                        };
            var result = runtime.Compute(memory, new[] { input });

            // ASSERT
            Assert.AreEqual(3, result.Length);

            Assert.AreEqual(0.0117020626F, result[0]);
            Assert.AreEqual(0.953143001F, result[1]);
            Assert.AreEqual(0.0351549424F, result[2]);
            Assert.AreEqual(1, result.ToArray().Sum(), 0.000001);
        }
    }
}