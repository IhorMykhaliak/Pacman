using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    class Inky : Ghost
    {
        private Blinky _blinky;

        public Blinky Blinky
        {
            set
            {
                _blinky = value;
            }
        }

        public Inky(Grid grid, int x, int y, float size)
            : base(grid, x, y, size)
        {
            _name = "Inky";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[21, 27], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[21, 27], _level.Map[23, 30], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[23, 30], StartCell, _level.Map));
        }

        public override void UpdateChasePath(Player pacman)
        {
            List<Cell> bestPath = AStarAlgorithm.CalculatePath(CurrentCell(), CalculateTargetCell(pacman), _level.Map);
            _pathIterator = 0;

            if (bestPath.Count >= chasePathLength)
            {
                _chasePath = bestPath.GetRange(0, chasePathLength);
            }
            else
            {
                _chasePath = bestPath;
            }

            _targetCell = _chasePath.Last();
        }

        private Cell CalculateOffsetCell(Player pacman)
        {
            int distance = 2;
            Cell offsetCell = new Cell();
            offsetCell.Content = Content.Wall;

            while (offsetCell.IsWall())
            {
                switch (pacman.Direction)
                {
                    case Direction.Up: offsetCell = _level.Map[pacman.GetX(), pacman.GetY() - distance];
                        break;
                    case Direction.Down: offsetCell = _level.Map[pacman.GetX(), pacman.GetY() + distance];
                        break;
                    case Direction.Left:
                        {
                            if (pacman.GetX() - distance > 0)
                            {
                                offsetCell = _level.Map[pacman.GetX() - distance, pacman.GetY()];
                            }
                            break;
                        }
                    case Direction.Right:
                        {
                            if (pacman.GetX() + distance < _level.Width - 1)
                            {
                                offsetCell = _level.Map[pacman.GetX() + distance, pacman.GetY()];
                            }
                            break;
                        }
                    default: offsetCell = pacman.CurrentCell();
                        break;
                }

                if (distance == 0)
                {
                    offsetCell = pacman.CurrentCell();
                    break;
                }

                distance--;
            }

            return offsetCell;
        }

        private Cell CalculateTargetCell(Player pacman)
        {
            Cell offsetCell = CalculateOffsetCell(pacman);
            Cell cell;
            int xDistance, yDistance;

            xDistance = pacman.GetX() - _blinky.GetX();
            yDistance = pacman.GetY() - _blinky.GetY();

            TargetInBounds(pacman, ref xDistance, ref yDistance);

            cell = _level.Map[pacman.GetX() + xDistance, pacman.GetY() + yDistance];

            return TargetEmpty(cell);
        }

        private void TargetInBounds(Player pacman, ref int xDistance, ref int yDistance)
        {
            while (pacman.GetX() + xDistance < 0)
            {
                xDistance++;
            }

            while (pacman.GetX() + xDistance > _level.Width - 1)
            {
                xDistance--;
            }

            while (pacman.GetY() + yDistance < 0)
            {
                yDistance++;
            }

            while (pacman.GetY() + yDistance > _level.Height - 1)
            {
                yDistance--;
            }
        }

        private Cell TargetEmpty(Cell cell)
        {
            int xShift = 0, yShift = 0;
            int upper;
            int count = 1;

            while (cell.IsWall())
            {
                if (count % 2 == 0)
                {
                    xShift = count / 2;
                    yShift = -xShift;
                    upper = xShift;
                }
                else
                {
                    xShift = -count / 2;
                    upper = -xShift;
                    yShift = xShift;
                }

                for (; yShift <= upper; yShift++)
                {
                    if (cell.GetX() + xShift > 0 && cell.GetX() + xShift < _level.Width &&
                        cell.GetY() + yShift > 0 && cell.GetY() + yShift < _level.Height)
                    {
                        cell = _level.Map[cell.GetX() + xShift, cell.GetY() + yShift];
                        if (!cell.IsWall())
                        {
                            return cell;
                        }
                    }
                }

                count++;
            }

            return cell;
        }
    }
}
