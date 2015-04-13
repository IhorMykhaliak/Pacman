using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test.Players.Pacman
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void TestPickItem1()
        {
            Game game = new Game();
            Player pacman = game.Player;
            pacman.SetX(pacman.GetX() - 1);

            pacman.PickItem(game.Ghosts);

            Assert.AreEqual(game.Score, 10);
            Assert.AreEqual(pacman.Coins, 1);
        }

        [TestMethod]
        public void TestPickItem2()
        {
            Game game = new Game();
            Player pacman = game.Player;
            pacman.SetX(pacman.GetX() - 11);

            pacman.PickItem(game.Ghosts);

            Assert.IsTrue(pacman.IsPoweredUp);
            Assert.AreEqual(pacman.Coins, 1);
            Assert.AreEqual(game.Score, 10);
        }
    }
}
