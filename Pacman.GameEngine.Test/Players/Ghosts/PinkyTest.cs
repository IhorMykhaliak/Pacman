using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test.Players.Ghosts.Base
{
    [TestClass]
    public class PinkyTest
    {
        [TestMethod]
        public void TestCalculateTargetCell1()
        {
            Game game = new Game();
            Player pacman = game.Player;
            Pinky pinky = (Pinky)game.Ghosts[1];
            pacman.Direction = Direction.Left;

            Cell cell = pinky.CalculateTargetCell();

            Assert.AreEqual(cell.GetX(), pacman.GetX() - 3);
            Assert.IsFalse(cell.IsWall());
        }

        [TestMethod]
        public void TestCalculateTargetCell2()
        {
            Game game = new Game();
            Player pacman = game.Player;
            Pinky pinky = (Pinky)game.Ghosts[1];
            pacman.Direction = Direction.Up;

            Cell cell = pinky.CalculateTargetCell();

            Assert.AreEqual(cell.GetX(), pacman.GetX());
            Assert.IsFalse(cell.IsWall());
        }
    }
}
