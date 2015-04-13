using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pacman.GameEngine;
using System.Collections.Generic;

namespace Pacman.GameEngine.Test
{
    [TestClass]
    public class GameTest
    {
        #region Initialization

        [TestMethod]
        public void TestInitializePacman()
        {
            Game game = new Game();

            Player pacman = game.Player;

            Assert.AreEqual(pacman.Size, 20);
            Assert.AreEqual(pacman.GetX() + 1, 17);
            Assert.AreEqual(pacman.GetY() + 1, 25);
        }

        [TestMethod]
        public void TestInitializeGhosts()
        {
            Game game = new Game();

            List<Ghost> ghosts = game.Ghosts;

            foreach (Ghost ghost in ghosts)
            {
                Assert.AreEqual(ghost.Size, 20);
                Assert.AreEqual(ghost.Behaviour, Behaviour.Return);
            }
        }

        [TestMethod]
        public void TestInitializeGhosts0()
        {
            Game game = new Game();

            Ghost ghost = game.Ghosts[0];

            Assert.AreEqual(ghost.GetX() + 1, 18);
            Assert.AreEqual(ghost.GetY() + 1, 15);

            Assert.AreEqual(ghost.GetX(), ghost.HomeCell.GetX());
            Assert.AreEqual(ghost.GetY(), ghost.HomeCell.GetY());

            Assert.AreEqual(ghost.StartCell.GetX(), 27);
            Assert.AreEqual(ghost.StartCell.GetY(), 2);
        }

        [TestMethod]
        public void TestInitializeGhosts1()
        {
            Game game = new Game();

            Ghost ghost = game.Ghosts[1];

            Assert.AreEqual(ghost.GetX() + 1, 17);
            Assert.AreEqual(ghost.GetY() + 1, 15);

            Assert.AreEqual(ghost.GetX(), ghost.HomeCell.GetX());
            Assert.AreEqual(ghost.GetY(), ghost.HomeCell.GetY());

            Assert.AreEqual(ghost.StartCell.GetX(), 6);
            Assert.AreEqual(ghost.StartCell.GetY(), 2);
        }

        [TestMethod]
        public void TestInitializeGhosts2()
        {
            Game game = new Game();

            Ghost ghost = game.Ghosts[2];

            Assert.AreEqual(ghost.GetX() + 1, 18);
            Assert.AreEqual(ghost.GetY() + 1, 14);

            Assert.AreEqual(ghost.GetX(), ghost.HomeCell.GetX());
            Assert.AreEqual(ghost.GetY(), ghost.HomeCell.GetY());

            Assert.AreEqual(ghost.StartCell.GetX(), 27);
            Assert.AreEqual(ghost.StartCell.GetY(), 27);
        }

        [TestMethod]
        public void TestInitializeGhosts3()
        {
            Game game = new Game();

            Ghost ghost = game.Ghosts[3];

            Assert.AreEqual(ghost.GetX() + 1, 17);
            Assert.AreEqual(ghost.GetY() + 1, 14);

            Assert.AreEqual(ghost.GetX(), ghost.HomeCell.GetX());
            Assert.AreEqual(ghost.GetY(), ghost.HomeCell.GetY());

            Assert.AreEqual(ghost.StartCell.GetX(), 4);
            Assert.AreEqual(ghost.StartCell.GetY(), 27);
        }

        [TestMethod]
        public void TestInitializeTimer()
        {
            Game game = new Game();

            System.Timers.Timer timer = game.MainTimer;

            Assert.AreEqual(timer.Interval, 10);
            Assert.IsFalse(timer.Enabled);
        }

        [TestMethod]
        public void TestInitializeGame()
        {
            Game game = new Game();

            Assert.IsTrue(game.IsPaused);
            Assert.AreEqual(game.ElapsedSeconds, 0.0f);
            Assert.AreEqual(game.Score, 0);
        }

        #endregion

        #region Event handlers

        [TestMethod]
        public void TestPauseGame()
        {
            Game game = new Game();

            game.PauseGame();

            Assert.IsFalse(game.IsPaused);
            Assert.IsTrue(game.MainTimer.Enabled);
        }

        #endregion
    }
}
