using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Pacman.GameEngine.Test
{
    [TestClass]
    public class AStarAlgorithmTest
    {
        [TestMethod]
        public void TestCalculatePath1()
        {
            Game game = new Game();
            Cell[,] map = game.Level.Map;
            Cell start = game.Ghosts[0].HomeCell;
            Cell end = game.Ghosts[0].StartCell;
            List<Cell> expectedPath = new List<Cell> 
            {
                map[17, 14],
                map[17, 13],
                map[17, 12],
                map[18, 12],
                map[18, 11],
                map[18, 10],
                map[18, 9],
                map[19, 9],
                map[20, 9],
                map[21, 9],
                map[21, 8],
                map[21, 7],
                map[21, 6],
                map[22, 6],
                map[23, 6],
                map[24, 6],
                map[24, 5],
                map[24, 4],
                map[24, 3],
                map[24, 2],
                map[25, 2],
                map[26, 2],
                map[27, 2]
            };

            IPathfindingAlgorithm algorithm = new AStarAlgorithm();
            List<Cell> path = algorithm.CalculatePath(start, end, map);

            CollectionAssert.AreEqual(expectedPath, path);
        }

        [TestMethod]
        public void TestCalculatePath2()
        {
            Game game = new Game();
            Cell[,] map = game.Level.Map;
            Cell start = map[21, 14];
            Cell end = map[27, 27];
            List<Cell> expectedPath = new List<Cell> 
            {
                map[21, 14],
                map[21, 15],
                map[21, 16],
                map[21, 17],
                map[21, 18],
                map[21, 19],
                map[21, 20],
                map[21, 21],
                map[22, 21],
                map[23, 21],
                map[24, 21],
                map[24, 22],
                map[24, 23],
                map[24, 24],
                map[24, 25],
                map[24, 26],
                map[24, 27],
                map[25, 27],
                map[26, 27],
                map[27, 27]
            };

            IPathfindingAlgorithm algorithm = new AStarAlgorithm();
            List<Cell> path = algorithm.CalculatePath(start, end, map);

            CollectionAssert.AllItemsAreNotNull(path);
            CollectionAssert.AllItemsAreUnique(path);
            CollectionAssert.AreEqual(expectedPath, path);
        }

        [TestMethod]
        public void TestCalculatePath3()
        {
            Game game = new Game();
            Cell[,] map = game.Level.Map;
            Cell start = game.Ghosts[0].HomeCell;
            Cell end = game.Ghosts[0].StartCell;
            List<Cell> expectedPath = new List<Cell> 
            {
                map[17, 14],
                map[17, 13],
                map[17, 12],
                map[18, 12],
                map[18, 11],
                map[18, 10],
                map[18, 9],
                map[19, 9],
                map[20, 9],
                map[21, 9],
                map[21, 8],
                map[21, 7],
                map[21, 6],
                map[22, 6],
                map[23, 6],
                map[24, 6],
                map[24, 5],
                map[24, 4],
                map[24, 3],
                map[24, 2],
                map[25, 2],
                map[26, 2],
                map[27, 2]
            };

            IPathfindingAlgorithm algorithm = new AStarAlgorithm();
            List<Cell> path = algorithm.CalculatePath(start, end, map);

            CollectionAssert.AllItemsAreNotNull(path);
            CollectionAssert.AllItemsAreUnique(path);
        }
    }
}
