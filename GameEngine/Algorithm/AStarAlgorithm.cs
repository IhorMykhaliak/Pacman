using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman.GameEngine
{
    public class AStarAlgorithm : IPathfindingAlgorithm
    {
        #region Fields

        private Cell _start;
        private Cell _end;
        private Cell[,] _grid;
        private List<Cell> _openedList;
        private List<Cell> _closedList;

        #endregion

        #region Calculation

        public List<Cell> CalculatePath(Cell startCell, Cell endCell, Cell[,] levelGrid)
        {
            // Review IM : Для покращення якості алгоритму варто явно обробляти випадок коли
            //             statCell == endCell, а не проходити алгоритмом все поле,щоб це дізнатися : General Coding
            Cell currentCell;
            List<Cell> adjacenedCells;

            _start = startCell;
            _end = endCell;
            _grid = levelGrid;

            _openedList = new List<Cell>();
            _closedList = new List<Cell>();

            _openedList.Add(_start);

            while (!_openedList.Contains(_end))
            {
                if (_openedList.Count == 0)
                {
                    // Review IM : Якщо логіка гри не передбачає випадку,коли шлях не існує,
                    //             краще створювати Exception                                : General Coding
                    break;
                }

                currentCell = GetBestCell();

                if (!_closedList.Contains(currentCell))
                {
                    _closedList.Add(currentCell);
                }

                _openedList.Remove(currentCell);

                adjacenedCells = AdjacenedCellsCheck(currentCell);
                _openedList.AddRange(adjacenedCells);
            }

            return GetResultPath();
        }

        private List<Cell> GetResultPath()
        {
            Cell temp = _end;
            List<Cell> _resultPath = new List<Cell>();
            while (temp != _start)
            {
                _resultPath.Add(temp);
                temp = temp.Parent;
            }

            _resultPath.Add(_start);
            _resultPath.Reverse();

            return _resultPath;
        }

        private Cell GetBestCell()
        {
            Cell bestCell;
            int minHeuristics;

            foreach (var c in _openedList)
            {
                c.ManhattanHeuristics = CalculateManhattanHeuristics(c, _end);
            }

            bestCell = _openedList.ElementAt(0);
            minHeuristics = _openedList.ElementAt(0).ManhattanHeuristics;

            foreach (var c in _openedList)
            {
                if (c.ManhattanHeuristics < minHeuristics)
                {
                    minHeuristics = c.ManhattanHeuristics;
                    bestCell = c;
                }
            }

            return bestCell;
        }

        private int CalculateManhattanHeuristics(Cell cell1, Cell cell2)
        {
            return Math.Abs(cell1.GetX() - cell2.GetX()) + Math.Abs(cell1.GetY() - cell2.GetY());
        }

        #endregion

        #region Check adjacened cells

        private List<Cell> AdjacenedCellsCheck(Cell cell)
        {
            List<Cell> adjacenedCells = new List<Cell>();

            CheckTopCell(cell, ref adjacenedCells);

            CheckBottomCell(cell, ref adjacenedCells);

            CheckLeftCell(cell, ref adjacenedCells);

            CheckRightCell(cell, ref adjacenedCells);

            return adjacenedCells;
        }

        private void CheckTopCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell topCell;

            if (cell.GetY() - 1 > 0)
            {
                topCell = _grid[cell.GetX(), cell.GetY() - 1];
                if (!topCell.IsWall() && !_closedList.Contains(topCell))
                {
                    topCell.Parent = cell;
                    adjacenedCells.Add(topCell);
                }
            }
        }

        private void CheckBottomCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell bottomCell;

            if (cell.GetY() < _grid.GetLength(0))
            {
                bottomCell = _grid[cell.GetX(), cell.GetY() + 1];
                if (!bottomCell.IsWall() && !_closedList.Contains(bottomCell))
                {
                    bottomCell.Parent = cell;
                    adjacenedCells.Add(bottomCell);
                }
            }
        }

        private void CheckLeftCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell leftCell;

            if (cell.GetX() > 0)
            {
                leftCell = _grid[cell.GetX() - 1, cell.GetY()];
                if (!leftCell.IsWall() && !_closedList.Contains(leftCell))
                {
                    leftCell.Parent = cell;
                    adjacenedCells.Add(leftCell);
                }
            }
        }

        private void CheckRightCell(Cell cell, ref List<Cell> adjacenedCells)
        {
            Cell rightCell;

            if (cell.GetX() < _grid.GetLength(1) - 1)
            {
                rightCell = _grid[cell.GetX() + 1, cell.GetY()];
                if (!rightCell.IsWall() && !_closedList.Contains(rightCell))
                {
                    rightCell.Parent = cell;
                    adjacenedCells.Add(rightCell);
                }
            }
        }

        #endregion
    }
}
