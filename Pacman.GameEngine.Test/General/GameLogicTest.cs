using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Pacman.GameEngine.Test
{
    [TestClass]
    public class GameLogicTest
    {
        #region Initialization

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitializeWithInvalidPacman()
        {
            GameLogic logic = new GameLogic(null, new List<Ghost>(), new Grid(), 1.0f);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitializeWithInvalidGhosts()
        {
            GameLogic logic = new GameLogic(new Player(), null, new Grid(), 1.0f);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInitializeWithInvalidGrid()
        {
            GameLogic logic = new GameLogic(new Player(), new List<Ghost>(), null, 1.0f);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInitializeWithInvalidDeltaTime()
        {
            GameLogic logic = new GameLogic(new Player(), new List<Ghost>(), new Grid(), -1.0f);
        }

        #endregion

        #region Pacman behaviour

        [TestMethod]
        public void TestPowerUpCheck()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            Player pacman = game.Player;
            List<Ghost> ghosts = game.Ghosts;
            float initialTime = pacman.PowerUpTime;

            pacman.IsPoweredUp = true;
            logic.PowerUpCheck();

            Assert.IsTrue(pacman.PowerUpTime < initialTime);
            foreach (var ghost in ghosts)
            {
                Assert.AreEqual(ghost.Behaviour, Behaviour.Frightened);
            }
        }

        [TestMethod]
        public void TestPowerDown()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            Player pacman = game.Player;
            List<Ghost> ghosts = game.Ghosts;
            pacman.PowerUpTime = - 1.0f;

            pacman.IsPoweredUp = true;
            logic.PowerUpCheck();

            Assert.IsFalse(pacman.IsPoweredUp);
            foreach (var ghost in ghosts)
            {
                Assert.AreEqual(ghost.TargetCell, ghost.CurrentCell());
                Assert.AreEqual(ghost.Behaviour, Behaviour.Chase);
            }
        }

        #endregion

        #region Ghosts behaviour

        [TestMethod]
        public void TestGhostDie()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            List<Ghost> ghosts = game.Ghosts;

            foreach (var ghost in ghosts)
            {
                logic.GhostDie(ghost);
            }

            foreach (var ghost in ghosts)
            {
                Assert.AreEqual(ghost.GetX(), ghost.HomeCell.GetX());
                Assert.AreEqual(ghost.GetY(), ghost.HomeCell.GetY());
                Assert.AreEqual(ghost.Direction, Direction.None);
                Assert.AreEqual(ghost.TargetCell, ghost.CurrentCell());
            }
        }

        [TestMethod]
        public void TestGhostCollisionCheck()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            List<Ghost> ghosts = game.Ghosts;
            Player pacman = game.Player;

            foreach (var ghost in ghosts)
            {
                ghost.SetX(pacman.GetX() + 1);
                ghost.SetY(pacman.GetY() + 1);
                ghost.Behaviour = Behaviour.Frightened;
                logic.GhostCollisionCheck(ghost);
            }

            foreach (var ghost in ghosts)
            {
                Assert.AreEqual(ghost.GetX(), ghost.HomeCell.GetX());
                Assert.AreEqual(ghost.GetY(), ghost.HomeCell.GetY());
                Assert.AreEqual(ghost.Direction, Direction.None);
                Assert.AreEqual(ghost.TargetCell, ghost.CurrentCell());
            }
        }

        [TestMethod]
        public void TestGhostPatroling()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            Ghost ghost = game.Ghosts[0];
            float initialTime = ghost.PatrolTime;

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Patrol;

            logic.GhostBehaviourCheck(ghost);

            Assert.IsTrue(ghost.PatrolTime < initialTime);
        }

        [TestMethod]
        public void TestGhostFrightened()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            Ghost ghost = game.Ghosts[0];

            game.Player.IsPoweredUp = true;
            ghost.Behaviour = Behaviour.Frightened;
            ghost.TargetCell = ghost.CurrentCell();

            logic.GhostBehaviourCheck(ghost);

            Assert.IsNotNull(ghost.RunPath);
        }

        [TestMethod]
        public void TestGhostChasing()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            Ghost ghost = game.Ghosts[0];
            float initialTime = ghost.ChaseTime;

            ghost.Behaviour = Behaviour.Chase;
            ghost.TargetCell = ghost.CurrentCell();

            logic.GhostBehaviourCheck(ghost);

            Assert.IsTrue(ghost.ChaseTime < initialTime);
            Assert.IsNotNull(ghost.ChasePath);
        }

        [TestMethod]
        public void TestGhostReturning()
        {
            Game game = new Game();
            GameLogic logic = game.Logic;
            Ghost ghost = game.Ghosts[0];
            float initialTime = ghost.ReturnTime;

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Return;

            logic.GhostBehaviourCheck(ghost);

            Assert.IsTrue(ghost.ReturnTime < initialTime);
            Assert.IsNotNull(ghost.ReturnPath);
        }

        #endregion
    }
}
