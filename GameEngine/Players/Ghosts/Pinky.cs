using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    class Pinky : Ghost
    {
        public Pinky(Grid grid, int x, int y, float size)
            : base(grid, x, y, size)
        {
            _name = "Pinky";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[8, 6], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[8, 6], _level.Map[4, 6], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[4, 6], StartCell, _level.Map));
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

        private Cell CalculateTargetCell(Player pacman)
        {
            int distance = 3;
            Cell cell = new Cell();
            cell.Content = Content.Wall;

            while (cell.IsWall())
            {
                switch (pacman.Direction)
                {
                    case Direction.Up:
                        {
                            if (pacman.GetY() - distance > 0)
                            {
                                cell = _level.Map[pacman.GetX(), pacman.GetY() - distance];
                            }
                            break;
                        }
                    case Direction.Down:
                        {
                            if (pacman.GetY() + distance < _level.Height - 1)
                            {
                                cell = _level.Map[pacman.GetX(), pacman.GetY() + distance];
                            }
                            break;
                        }
                    case Direction.Left:
                        {
                            if (pacman.GetX() - distance > 0)
                            {
                                cell = _level.Map[pacman.GetX() - distance, pacman.GetY()];
                            }
                            break;
                        }
                    case Direction.Right:
                        {
                            if (pacman.GetX() + distance < _level.Width - 1)
                            {
                                cell = _level.Map[pacman.GetX() + distance, pacman.GetY()];
                            }
                            break;
                        }
                    default: return pacman.CurrentCell();
                }

                if (distance <= 1)
                {
                    return pacman.CurrentCell();
                }

                distance--;
            }

            return cell;
        }
    }
}
