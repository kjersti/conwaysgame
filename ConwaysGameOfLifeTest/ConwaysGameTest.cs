using System;
using System.Collections.Generic;
using ConwaysGameOfLife;
using NUnit.Framework;

namespace ConwaysGameOfLifeTest
{
    [TestFixture]
    public class ConwaysGameTest
    {
        [Test]
        public void CanParseGridFromString()
        {
            var input = "Generation 1:" + Environment.NewLine
                        + "4 8" + Environment.NewLine
                        + "........" + Environment.NewLine
                        + "....*..." + Environment.NewLine
                        + "...**..." + Environment.NewLine
                        + "........" + Environment.NewLine;

            var actual = new GridParser().Parse(input);

            Assert.That(actual.IsAlive(1, 4));
            Assert.That(actual.IsAlive(2, 3));
            Assert.That(actual.IsAlive(2, 4));
            Assert.IsFalse(actual.IsAlive(0, 0));
            Assert.IsFalse(actual.IsAlive(1, 0));
            Assert.IsFalse(actual.IsAlive(3, 0));
        }

        [Test]
        public void CanParseSmallGridWithOneLineCell()
        {
            var input = "Generation 1:" + Environment.NewLine + "1 1" + Environment.NewLine + "*" + Environment.NewLine;

            Assert.That(new GridParser().Parse(input).IsAlive(0, 0));
        }

        [Test]
        public void CanGenerateNextGenerationWithOneCellGrid()
        {
            IList<Cell> cells = new List<Cell>(new[] { new Cell(0, 0, true) });            
            var grid = new Grid(1, cells, 1, 1);

            Assert.IsFalse(grid.Tick().IsAlive(0,0));
        }

        [Test]
        public void CanGenerateNextGenerationWithLargerGrid()
        {
            IList<Cell> cells = new List<Cell>(new[] { new Cell(1, 4, true), new Cell(2, 3, true), new Cell(2, 4, true)});            
            var grid = new Grid(1, cells, 4, 8);

            var nextGen = grid.Tick();
            Assert.That(nextGen.IsAlive(1, 3));
            Assert.That(nextGen.IsAlive(1, 4));
            Assert.That(nextGen.IsAlive(2, 3));
            Assert.That(nextGen.IsAlive(2, 4));
        }

        [Test]
        public void CanGenerateNextGenerationWhereCellDies()
        {
            IList<Cell> cells = new List<Cell>(new[] { new Cell(0, 0, true)});
            var grid = new Grid(1, cells, 4, 8);

            Assert.IsFalse(grid.Tick().IsAlive(0, 0));
        }
    }
}