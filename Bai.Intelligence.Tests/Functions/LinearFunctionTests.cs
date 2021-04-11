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
    public class LinearFunctionTests
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
            var function = new LinearFunction();

            // ACT
            var result = function.Compute(0.5F);

            // ASSERT
            Assert.AreEqual(0.5F, result);
        }
    }
}
