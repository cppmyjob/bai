using System;
using Bai.Intelligence.Organism.Functions;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Functions
{
    public class SigmoidFunctionTests
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
            var function = new SigmoidFunction {Alfa = 4.4F};

            // ACT
            var result = function.Compute(0.5F);

            // ASSERT
            var expectValue = (float) (1.0 / (1.0 + Math.Exp(-4.4F * 0.5F)));
            Assert.AreEqual(expectValue, result);
        }
    }
}
