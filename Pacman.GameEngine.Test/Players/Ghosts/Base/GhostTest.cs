using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test.Players.Ghosts.Base
{
    [TestClass]
    public class GhostTest
    {
        #region Movement

        [TestMethod]
        public void TestFollowPath1()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Patrol;

            ghost.Patrol();

            Assert.AreEqual(ghost.CurrentCell().GetX(), 26);
            Assert.AreEqual(ghost.CurrentCell().GetY(), 1);
        }

        [TestMethod]
        public void TestFollowPath2()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[1];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Patrol;

            ghost.Patrol();

            Assert.AreEqual(ghost.CurrentCell().GetX(), 5);
            Assert.AreEqual(ghost.CurrentCell().GetY(), 1);
        }

        #endregion

        #region Movement constraints

        [TestMethod]
        public void TestIsUpAvailable()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX() + 1);
            ghost.SetY(ghost.StartCell.GetY() + 1);

            Assert.IsTrue(ghost.IsUpAvailable());
        }

        [TestMethod]
        public void TestIsDownAvailable()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX() + 1);
            ghost.SetY(ghost.StartCell.GetY() + 1);

            Assert.IsTrue(ghost.IsDownAvailable());
        }

        [TestMethod]
        public void TestIsLeftAvailable()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());

            Assert.IsTrue(ghost.IsLeftAvailable());
        }

        [TestMethod]
        public void TestIsRightAvailable()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());

            Assert.IsTrue(ghost.IsRightAvailable());
        }

        #endregion

        #region Update direction

        [TestMethod]
        public void TestTryDirectionUp()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.UpdateDirection(game.Level.Map[ghost.GetX(), ghost.GetY() - 1]);

            Assert.AreEqual(ghost.Direction, Direction.Up);
        }

        [TestMethod]
        public void TestTryDirectionDown()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.UpdateDirection(game.Level.Map[ghost.GetX(), ghost.GetY() + 1]);

            Assert.AreEqual(ghost.Direction, Direction.Down);
        }

        [TestMethod]
        public void TestTryDirectionLeft()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.UpdateDirection(game.Level.Map[ghost.GetX() - 1, ghost.GetY()]);

            Assert.AreEqual(ghost.Direction, Direction.Left);
        }

        [TestMethod]
        public void TestTryDirectionRight()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.UpdateDirection(game.Level.Map[ghost.GetX() + 1, ghost.GetY()]);

            Assert.AreEqual(ghost.Direction, Direction.Right);
        }

        #endregion

        #region Behaviour

        [TestMethod]
        public void TestDoPatroling1()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Patrol;
            ghost.PatrolTime = 1.0f;

            ghost.DoPatroling();

            Assert.AreEqual(ghost.CurrentCell().GetX(), 26);
            Assert.AreEqual(ghost.CurrentCell().GetY(), 1);
        }

        [TestMethod]
        public void TestDoPatroling2()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Patrol;
            ghost.PatrolTime = 0.0f;

            ghost.DoPatroling();

            Assert.AreEqual(ghost.CurrentCell(), ghost.TargetCell);
            Assert.AreEqual(ghost.ChaseTime, 20.0f);
            Assert.AreEqual(ghost.Behaviour, Behaviour.Chase);
        }

        [TestMethod]
        public void TestDoChasing1()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Chase;
            ghost.ChaseTime = 1.0f;
            ghost.ChasePath = AStarAlgorithm.CalculatePath(ghost.CurrentCell(), game.Player.CurrentCell(), game.Level.Map);

            for (int i = 0; i < 10; i++)
            {
                ghost.DoChasing();
            }

            Assert.AreEqual(ghost.CurrentCell().GetX(), 26);
            Assert.AreEqual(ghost.CurrentCell().GetY(), 2);
        }

        [TestMethod]
        public void TestDoChasing2()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.SetX(ghost.StartCell.GetX());
            ghost.SetY(ghost.StartCell.GetY());
            ghost.Behaviour = Behaviour.Chase;
            ghost.ChaseTime = 0.0f;

            for (int i = 0; i < 10; i++)
            {
                ghost.DoChasing();
            }

            CollectionAssert.AreEqual(ghost.ReturnPath, AStarAlgorithm.CalculatePath(ghost.CurrentCell(), ghost.StartCell, game.Level.Map));
            Assert.AreEqual(ghost.Behaviour, Behaviour.Return);
        }

        [TestMethod]
        public void TestDoReturning()
        {
            Game game = new Game();
            Ghost ghost = game.Ghosts[0];

            ghost.Behaviour = Behaviour.Return;
            ghost.ReturnTime = 1.0f;

            for (int i = 0; i < 20; i++)
            {
                ghost.DoReturning();
            }

            Assert.AreEqual(ghost.CurrentCell().GetX(), 17);
            Assert.AreEqual(ghost.CurrentCell().GetY(), 12);
        }

        #endregion
    }
}
