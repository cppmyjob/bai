using System.Runtime.InteropServices.ComTypes;
using Bai.Intelligence.Cpu;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests
{
    public class Tests
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

            // ACT
            var builder = new CpuBuilder();
            var runtime = builder.Build(definition);

            var result = runtime.Compute(new[] {10F, 10F, 30F });

            // ASSERT
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(2.222, result[0]);
        }
    }
}