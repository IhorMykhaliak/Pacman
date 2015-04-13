using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test.Players.Ghosts
{
    [TestClass]
    public class InkyTest
    {
        [TestMethod]
        public void TestCalculateTargetCell1()
        {
            Game game = new Game();
            Inky inky = (Inky)game.Ghosts[2];
            Blinky blinky = (Blinky)game.Ghosts[0];
            blinky.SetX(10);
            blinky.SetY(16);

            Cell cell = inky.CalculateTargetCell();

            Assert.AreEqual(cell, game.Level.Map[27, 30]);
        }

        [TestMethod]
        public void TestCalculateTargetCell2()
        {
            Game game = new Game();
            Inky inky = (Inky)game.Ghosts[2];
            Blinky blinky = (Blinky)game.Ghosts[0];
            blinky.SetX(25);
            blinky.SetY(16);

            Cell cell = inky.CalculateTargetCell();

            Assert.AreEqual(cell, game.Level.Map[12, 30]);
        }
    }
}
