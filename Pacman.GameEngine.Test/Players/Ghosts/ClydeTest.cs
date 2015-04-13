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
            List<Cell> path = AStarAlgorithm.CalculatePath(clyde.CurrentCell(), pacman.CurrentCell(), game.Level.Map).GetRange(0, 3);

            clyde.UpdateChasePath();

            CollectionAssert.AreEqual(clyde.ChasePath, path);
        }
    }
}
