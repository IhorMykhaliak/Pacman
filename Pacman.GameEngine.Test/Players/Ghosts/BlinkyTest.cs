using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Pacman.GameEngine.Test.Players.Ghosts
{
    [TestClass]
    public class BlinkyTest
    {
        [TestMethod]
        public void TestUpdateChasePath1()
        {
            Game game = new Game();
            Blinky blinky = (Blinky)game.Ghosts[0];
            Player pacman = game.Player;
            pacman.IsPassedRightTunnel = true;

            blinky.UpdateChasePath();

            List<Cell> path = AStarAlgorithm.CalculatePath(blinky.CurrentCell(), game.Level.Map[33, 15], game.Level.Map);

            Assert.AreEqual(blinky.TargetCell, game.Level.Map[0, 15]);
            Assert.IsFalse(pacman.IsPassedRightTunnel);
            CollectionAssert.AreEqual(blinky.ChasePath, path);
        }

        [TestMethod]
        public void TestUpdateChasePath2()
        {
            Game game = new Game();
            Blinky blinky = (Blinky)game.Ghosts[0];
            Player pacman = game.Player;
            pacman.IsPassedLeftTunnel = true;

            blinky.UpdateChasePath();

            List<Cell> path = AStarAlgorithm.CalculatePath(blinky.CurrentCell(), game.Level.Map[0, 15], game.Level.Map);

            Assert.AreEqual(blinky.TargetCell, game.Level.Map[32, 15]);
            Assert.IsFalse(pacman.IsPassedLeftTunnel);
            CollectionAssert.AreEqual(blinky.ChasePath, path);
        }
    }
}
