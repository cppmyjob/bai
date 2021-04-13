using System.Linq;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Cpu
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
        public void ShouldSetCorrectInputValues()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);
            var memory = new[] { 10F, 20F, 30F };
            runtime.SetInputMemory(memory);

            // ACT
            var input = new RuntimeInput
            {
                Offset = 0,
                Length = memory.Length
            };
            var result = runtime.Compute(new[] { input });

            // ASSERT
            Assert.AreEqual(10F, runtime.TempMemory[0]);
            Assert.AreEqual(20F, runtime.TempMemory[1]);
            Assert.AreEqual(30F, runtime.TempMemory[2]);
        }

        [Test]
        public void ShouldCalculateSimpleNeuron()
        {
            // ARRANGE
            var definition = _env.CreateSimpleNeuron();
            var builder = new CpuBuilder();
            var runtime = (CpuRuntime)builder.Build(definition);
            var memory = new[] { 1F, 2F, 3F };
            runtime.SetInputMemory(memory);

            // ACT
            var input = new RuntimeInput
            {
                Offset = 0,
                Length = memory.Length
            };
            var result = runtime.Compute(new[] { input });

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
            runtime.SetInputMemory(memory);

            // ACT
            var input = new RuntimeInput
                        {
                            Offset = 0,
                            Length = memory.Length
                        };
            var result = runtime.Compute(new[] { input });

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
            runtime.SetInputMemory(memory);

            // ACT
            var input = new RuntimeInput
                        {
                            Offset = 0,
                            Length = memory.Length
                        };
            var result = runtime.Compute(new[] { input });

            // ASSERT
            Assert.AreEqual(3, result.Length);

            Assert.AreEqual(0.0117020626F, result[0]);
            Assert.AreEqual(0.953143001F, result[1]);
            Assert.AreEqual(0.0351549424F, result[2]);
            Assert.AreEqual(1, result.Sum(), 0.000001);
        }
    }
}