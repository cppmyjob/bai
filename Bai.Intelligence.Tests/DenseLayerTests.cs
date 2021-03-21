using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests
{
    public class DenseLayerTests
    {
        private TestEnvBase _env;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        [Test]
        public void ShouldCreateCorrectGenes()
        {
            // ARRANGE
            var dense = new Dense(3);

            // ACT
            var genes = dense.Compile();

            // ASSERT
            Assert.AreEqual(1, genes.Length);
        }

    }
}
