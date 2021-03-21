using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Models;
using Bai.Intelligence.Models.Layers;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests
{
    public class ModelCompileTests
    {
        private TestEnvBase _env;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        //[Test]
        //public void ShouldCreateCorrectGenes()
        //{
        //    // ARRANGE
        //    var model = new Sequential();
        //    model.Layers.Add(new Dense(3));

        //    // ACT
        //    model.Compile();

        //    // ASSERT
        //    Assert.IsNotNull(model.NetworkDefinition);
        //    Assert.AreEqual(3, model.NetworkDefinition.InputCount);
        //    Assert.AreEqual(1, model.NetworkDefinition.OutputCount);
        //}

    }
}
