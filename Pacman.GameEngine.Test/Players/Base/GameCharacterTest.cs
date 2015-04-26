using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test.Players.Base
{
    [TestClass]
    public class GameCharacterTest
    {
        #region Position

        [TestMethod]
        public void TestGetTop()
        {
            Game game = new Game();
            Player pacman = game.Player;

            Assert.AreEqual(pacman.GetTop(), 480.0f);
        }

        [TestMethod]
        public void TestGetBottom()
        {
            Game game = new Game();
            Player pacman = game.Player;

            Assert.AreEqual(pacman.GetBottom(), 500.0f);
        }

        [TestMethod]
        public void TestGetLeft()
        {
            Game game = new Game();
            Player pacman = game.Player;

            Assert.AreEqual(pacman.GetLeft(), 320.0f);
        }

        [TestMethod]
        public void TestGetRight()
        {
            Game game = new Game();
            Player pacman = game.Player;

            Assert.AreEqual(pacman.GetRight(), 340.0f);
        }

        #endregion

        #region Movement

        [TestMethod]
        public void TestMoveUp()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.Direction = Direction.Up;
            for (int i = 0; i < 10; i++)
            {
                ghost.Move();
            }

            Cell cell = ghost.CurrentCell();

            Assert.IsTrue(cell.IsNextTo(ghost.HomeCell));
            Assert.AreEqual(cell.GetY() - ghost.HomeCell.GetY(), -1);
            Assert.AreEqual(cell.GetX(), ghost.HomeCell.GetX());
        }

        [TestMethod]
        public void TestMoveDown()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.Direction = Direction.Down;
            for (int i = 0; i < 10; i++)
            {
                ghost.Move();
            }

            Cell cell = ghost.CurrentCell();

            Assert.IsTrue(cell.IsNextTo(ghost.HomeCell));
            Assert.AreEqual(cell.GetY() - ghost.HomeCell.GetY(), 1);
            Assert.AreEqual(cell.GetX(), ghost.HomeCell.GetX());
        }

        [TestMethod]
        public void TestMoveLeft()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.Direction = Direction.Left;
            for (int i = 0; i < 10; i++)
            {
                ghost.Move();
            }

            Cell cell = ghost.CurrentCell();

            Assert.IsTrue(cell.IsNextTo(ghost.HomeCell));
            Assert.AreEqual(cell.GetY(), ghost.HomeCell.GetY());
            Assert.AreEqual(cell.GetX() - ghost.HomeCell.GetX(), -1);
        }

        [TestMethod]
        public void TestMoveRight()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.Direction = Direction.Right;
            for (int i = 0; i < 10; i++)
            {
                ghost.Move();
            }

            Cell cell = ghost.CurrentCell();

            Assert.IsTrue(cell.IsNextTo(ghost.HomeCell));
            Assert.AreEqual(cell.GetY(), ghost.HomeCell.GetY());
            Assert.AreEqual(cell.GetX() - ghost.HomeCell.GetX(), 1);
        }

        [TestMethod]
        public void TestTryPassLeftTunnel()
        {
            Game game = new Game();
            Player pacman = game.Player;
            pacman.SetX(0);
            pacman.SetY(15);
            pacman.Direction = Direction.Left;

            pacman.TryPassLeftTunnel();

            Assert.IsTrue(pacman.IsPassedLeftTunnel);
            Assert.IsFalse(pacman.IsPassedRightTunnel);
            Assert.AreEqual(pacman.GetX() + 1, game.Level.Width - 1);
            Assert.AreEqual(pacman.GetY() + 1, 15);
        }
        
        [TestMethod]
        public void TestTryPassRightTunnel()
        {
            Game game = new Game();
            Player pacman = game.Player;
            pacman.SetX(game.Level.Width);
            pacman.SetY(15);
            pacman.Direction = Direction.Right;

            pacman.TryPassRightTunnel();

            Assert.IsTrue(pacman.IsPassedRightTunnel);
            Assert.IsFalse(pacman.IsPassedLeftTunnel);
            Assert.AreEqual(pacman.GetX() + 1, 1);
            Assert.AreEqual(pacman.GetY() + 1, 15);
        }

        [TestMethod]
        public void TestCanMoveFailed()
        {
            Game game = new Game();
            Player pacman = game.Player;
            Cell origin = pacman.CurrentCell();

            pacman.Move();

            Assert.AreEqual(origin, pacman.CurrentCell());
        }

        #endregion
    }
}
