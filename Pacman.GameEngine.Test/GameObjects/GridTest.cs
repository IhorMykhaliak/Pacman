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
            Assert.IsTrue(cell.GetX() + 1 < level.Width - 1);
            Assert.IsTrue(cell.GetY() + 1 < level.Height - 1);
        }
    }
}
