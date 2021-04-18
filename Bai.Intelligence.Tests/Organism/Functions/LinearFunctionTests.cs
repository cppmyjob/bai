using Bai.Intelligence.Organism.Functions;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Organism.Functions
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
