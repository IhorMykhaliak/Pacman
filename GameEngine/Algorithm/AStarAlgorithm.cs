using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public static class AStarAlgorithm
    {
        #region Fields

        private static Cell start;
        private static Cell end;
        private static Cell[,] grid;
        private static List<Cell> openedList;
        private static List<Cell> closedList;

        #endregion

        #region Calculation

        public static List<Cell> CalculatePath(Cell startCell, Cell endCell, Cell[,] levelGrid)
        {
            Cell currentCell;
            List<Cell> adjacenedCells;

            start = startCell;
            end = endCell;
            grid = levelGrid;

            openedList = new List<Cell>();
            closedList = new List<Cell>();

            openedList.Add(start);

            while (!openedList.Contains(end))
            {
                if (openedList.Count == 0)
                {
                    break;
                }

                currentCell = GetBestCell();

                if (!closedList.Contains(currentCell))
                {
                    closedList.Add(currentCell);
                }

                openedList.Remove(currentCell);

                adjacenedCells = AdjacenedCellsCheck(currentCell);
                openedList.AddRange(adjacenedCells);
            }

            return GetResultPath();
        }

        private static List<Cell> GetResultPath()
        {
            Cell temp = end;
            List<Cell> _resultPath = new List<Cell>();
            while (temp != start)
            {
                _resultPath.Add(temp);
                temp = temp.Parent;
            }

            _resultPath.Add(start);
            _resultPath.Reverse();

            return _resultPath;
        }

        private static Cell GetBestCell()
        {
            Cell bestCell;
            int minHeuristics;

            foreach (var c in openedList)
            {
                c.ManhattanHeuristics = CalculateManhattanHeuristics(c, end);
            }

            bestCell = openedList.ElementAt(0);
            minHeuristics = openedList.ElementAt(0).ManhattanHeuristics;

            foreach (var c in openedList)
            {
                if (c.ManhattanHeuristics < minHeuristics)
                {
                    minHeuristics = c.ManhattanHeuristics;
                    bestCell = c;
                }
            }

            return bestCell;
        }

        private static int CalculateManhattanHeuristics(Cell cell1, Cell cell2)
        {
            return Math.Abs(cell1.GetX() - cell2.GetX()) + Math.Abs(cell1.GetY() - cell2.GetY());
        }

        #endregion

        #region Check adjacened cells

        private static List<Cell> AdjacenedCellsCheck(Cell cell)
        {
            List<Cell> adjacenedCells = new List<Cell>();

            CheckTopCell(cell, ref adjacenedCells);

            CheckBottomCell(cell, ref adjacenedCells);

            CheckLeftCell(cell, ref adjacenedCells);

            CheckRightCell(cell, ref adjacenedCells);

            return adjacenedCells;
        }

        private static void CheckTopCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell topCell;

            if (cell.GetY() - 1 > 0)
            {
                topCell = grid[cell.GetX(), cell.GetY() - 1];
                if (!topCell.IsWall() && !closedList.Contains(topCell))
                {
                    topCell.Parent = cell;
                    adjacenedCells.Add(topCell);
                }
            }
        }

        private static void CheckBottomCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell bottomCell;

            if (cell.GetY() < grid.GetLength(0))
            {
                bottomCell = grid[cell.GetX(), cell.GetY() + 1];
                if (!bottomCell.IsWall() && !closedList.Contains(bottomCell))
                {
                    bottomCell.Parent = cell;
                    adjacenedCells.Add(bottomCell);
                }
            }
        }

        private static void CheckLeftCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell leftCell;

            if (cell.GetX() > 0)
            {
                leftCell = grid[cell.GetX() - 1, cell.GetY()];
                if (!leftCell.IsWall() && !closedList.Contains(leftCell))
                {
                    leftCell.Parent = cell;
                    adjacenedCells.Add(leftCell);
                }
            }
        }

        private static void CheckRightCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell rightCell;

            if (cell.GetX() < grid.GetLength(1) - 1)
            {
                rightCell = grid[cell.GetX() + 1, cell.GetY()];
                if (!rightCell.IsWall() && !closedList.Contains(rightCell))
                {
                    rightCell.Parent = cell;
                    adjacenedCells.Add(rightCell);
                }
            }
        }

        #endregion
    }
}
