using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public class Pinky : Ghost
    {
        #region Initialization

        public Pinky(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            _name = "Pinky";
            _distance = 3;
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[8, 6], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[8, 6], _level.Map[4, 6], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[4, 6], StartCell, _level.Map));
        }

        #endregion

        #region Behaviour

        public override Cell CalculateTargetCell()
        {
            Cell cell = new Cell();
            cell.Content = Content.Wall;

            while (cell.IsWall())
            {
                switch (_pacman.Direction)
                {
                    case Direction.Up:
                        {
                            if (IsUpAvailable())
                            {
                                cell = GetUpCell();
                            }
                            break;
                        }
                    case Direction.Down:
                        {
                            if (IsDownAvailable())
                            {
                                cell = GetDownCell();
                            }
                            break;
                        }
                    case Direction.Left:
                        {
                            if (IsLeftAvailable())
                            {
                                cell = GetLeftCell();
                            }
                            break;
                        }
                    case Direction.Right:
                        {
                            if (IsRightAvailable())
                            {
                                cell = GetRightCell();
                            }
                            break;
                        }
                    default: return _pacman.CurrentCell();
                }

                if (_distance <= 1)
                {
                    cell = _pacman.CurrentCell();
                    break;
                }

                _distance--;
            }

            return cell;
        }

        #endregion
    }
}
