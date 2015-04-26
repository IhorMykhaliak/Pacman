using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test.GameObjects
{
    [TestClass]
    public class GridTest
    {
        [TestMethod]
        public void TestGetRandomFreeCell()
        {
            Game game = new Game();
            Grid level = game.Level;

            Cell cell = level.GetRandomFreeCell();

            Assert.IsFalse(cell.IsWall());
        }

        [TestMethod]
        public void TestGetRandomFreeCellLoop()
        {
            Game game = new Game();
            Grid level = game.Level;

            Cell cell;
            for (int i = 0; i < 100; i++)
            {
                cell = level.GetRandomFreeCell();

                Assert.IsFalse(cell.IsWall());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInitializeWithInvalidSize()
        {
            Grid level = new Grid(new char[,] { { 'w', 'w' }, { 'w', 'w' } }, -0.5f);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInitializeWithInvalidMap()
        {
            Grid level = new Grid(new char[,] { { 'w', 'w' }, { 'w', 'w' }, { 'w', 'w' } }, 10.0f);
        }
    }
}
