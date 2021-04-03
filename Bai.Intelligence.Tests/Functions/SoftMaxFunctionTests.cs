using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Function;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Functions
{
    public class SoftMaxFunctionTests
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
            var function = new SoftMaxFunction();

            // ACT
            var inputValues = new float[] {1.3F, 5.1F, 2.2F, 0.7F, 1.1F};

            var result = function.Compute(inputValues);

            // ASSERT
            var expectValues = new [] { 0.02019046F, 0.90253769F, 0.04966053F, 0.01108076F, 0.01653055F };
            Assert.AreEqual(expectValues[0], result[0], 0.00000001);
            Assert.AreEqual(expectValues[1], result[1], 0.00000001);
            Assert.AreEqual(expectValues[2], result[2], 0.00000001);
            Assert.AreEqual(expectValues[3], result[3], 0.00000001);
            Assert.AreEqual(expectValues[4], result[4], 0.00000001);
        }
    }
}
