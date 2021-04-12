using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bai.Intelligence.Models;
using Bai.Intelligence.Tests.Infrastructure;
using NUnit.Framework;

namespace Bai.Intelligence.Tests.Models
{
    public class SequentialContextTests
    {
        private TestEnvBase _env;

        [SetUp]
        public void Setup()
        {
            _env = new TestEnvBase();
        }

        [Test]
        public void ShouldHasCorrectValuesAfterInitLayer()
        {
            // ARRANGE
            int firstInputCount = 2;
            int finalOutputCount = 4;
            int layersNumber = 3;

            // ACT
            var context = new SequentialContext(firstInputCount, finalOutputCount, layersNumber);
            context.Init();

            // ASSERT
            Assert.AreEqual(firstInputCount + finalOutputCount, context.OutputOffset);
            Assert.AreEqual(firstInputCount, context.PreviousInputCount);
            Assert.AreEqual(0, context.InputOffset);
        }


        [Test]
        public void ShouldHasCorrectValuesAfterFirstLayer()
        {
            // ARRANGE
            int firstInputCount = 2;
            int finalOutputCount = 4;
            int layersNumber = 3;

            // ACT
            var context = new SequentialContext(firstInputCount, finalOutputCount, layersNumber);
            context.Init();
            context.OutputOffset++;
            context.OutputOffset++;
            context.OutputOffset++;
            context.NextLayer(8);

            // ASSERT
            Assert.AreEqual(firstInputCount + finalOutputCount + 3, context.OutputOffset);
            Assert.AreEqual(8, context.PreviousInputCount);
            Assert.AreEqual(firstInputCount + finalOutputCount, context.InputOffset);
        }

        [Test]
        public void ShouldHasCorrectValuesOnLatestLayer()
        {
            // ARRANGE
            int firstInputCount = 2;
            int finalOutputCount = 4;
            int layersNumber = 3;

            // ACT
            var context = new SequentialContext(firstInputCount, finalOutputCount, layersNumber);
            context.Init();
            context.OutputOffset++;
            context.OutputOffset++;
            context.NextLayer(8);

            context.OutputOffset++;
            context.OutputOffset++;
            context.OutputOffset++;
            context.NextLayer(10);

            // ASSERT
            Assert.AreEqual(2, context.OutputOffset);
            Assert.AreEqual(10, context.PreviousInputCount);
            Assert.AreEqual(firstInputCount + finalOutputCount + 2, context.InputOffset);
        }


        [Test]
        public void ShouldHasCorrectValuesAfterInsertLayerWith1Layer()
        {
            // ARRANGE
            int firstInputCount = 2;
            int finalOutputCount = 4;
            int layersNumber = 1;

            // ACT
            var context = new SequentialContext(firstInputCount, finalOutputCount, layersNumber);
            context.Init();
            context.InsertLayer();

            // ASSERT
            Assert.AreEqual(firstInputCount + finalOutputCount, context.OutputOffset);
            Assert.AreEqual(firstInputCount, context.PreviousInputCount);
            Assert.AreEqual(0, context.InputOffset);
        }

        [Test]
        public void ShouldHasCorrectValuesAfterInsertAndNextLayerWith1Layer()
        {
            // ARRANGE
            int firstInputCount = 2;
            int finalOutputCount = 4;
            int layersNumber = 1;

            // ACT
            var context = new SequentialContext(firstInputCount, finalOutputCount, layersNumber);
            context.Init();
            context.InsertLayer();
            context.OutputOffset++;
            context.OutputOffset++;
            context.OutputOffset++;
            context.NextLayer(77);

            // ASSERT
            Assert.AreEqual(firstInputCount, context.OutputOffset);
            Assert.AreEqual(77, context.PreviousInputCount);
            Assert.AreEqual(6, context.InputOffset);
        }


        [Test]
        public void ShouldHasCorrectValuesAfterInsertLayerWith2Layers()
        {
            // ARRANGE
            int firstInputCount = 2;
            int finalOutputCount = 4;
            int layersNumber = 2;

            // ACT
            var context = new SequentialContext(firstInputCount, finalOutputCount, layersNumber);
            context.Init();
            context.InsertLayer();

            // ASSERT
            Assert.AreEqual(firstInputCount + finalOutputCount, context.OutputOffset);
            Assert.AreEqual(firstInputCount, context.PreviousInputCount);
            Assert.AreEqual(0, context.InputOffset);
        }

        [Test]
        public void ShouldHasCorrectValuesAfterInsertAndNextLayerWith2Layers()
        {
            // ARRANGE
            int firstInputCount = 2;
            int finalOutputCount = 4;
            int layersNumber = 2;

            // ACT
            var context = new SequentialContext(firstInputCount, finalOutputCount, layersNumber);
            context.Init();
            context.InsertLayer();
            context.OutputOffset++;
            context.OutputOffset++;
            context.OutputOffset++;
            context.NextLayer(77);

            // ASSERT
            Assert.AreEqual(firstInputCount + finalOutputCount + 3, context.OutputOffset);
            Assert.AreEqual(77, context.PreviousInputCount);
            Assert.AreEqual(6, context.InputOffset);
        }
    }
}
