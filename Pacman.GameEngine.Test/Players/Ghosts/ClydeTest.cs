using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Pacman.GameEngine.Test.Players.Ghosts
{
    [TestClass]
    public class ClydeTest
    {
        [TestMethod]
        public void TestUpdateChasePath()
        {
            Game game = new Game();
            Player pacman = game.Player;
            Clyde clyde = (Clyde)game.Ghosts[3];
            clyde.SetX(4);
            clyde.SetY(4);
            pacman.SetX(28);
            pacman.SetY(3);

            IPathfindingAlgorithm algorithm = new AStarAlgorithm();
            List<Cell> path = algorithm.CalculatePath(clyde.CurrentCell(), pacman.CurrentCell(), game.Level.Map).GetRange(0, 3);

            clyde.UpdateChasePath();

            CollectionAssert.AreEqual(clyde.ChasePath, path);
        }

        [TestMethod]
        public void TestUseStupidPath()
        {
            Game game = new Game();
            Player pacman = game.Player;
            Clyde clyde = (Clyde)game.Ghosts[3];
            clyde.SetX(4);
            clyde.SetY(4);
            pacman.SetX(28);
            pacman.SetY(3);
            IPathfindingAlgorithm algorithm = new AStarAlgorithm();
            List<Cell> path = algorithm.CalculatePath(clyde.CurrentCell(), pacman.CurrentCell(), game.Level.Map).GetRange(0, 3);

            clyde.UseStupidPath();

            Assert.AreEqual(clyde.ChasePath.Count, 3);
        }
    }
}
