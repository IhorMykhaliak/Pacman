using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    class Pinky : Ghost
    {
        #region Fields

        private int _distance = 3;

        #endregion

        #region Initialization

        public Pinky(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            _name = "Pinky";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[8, 6], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[8, 6], _level.Map[4, 6], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[4, 6], StartCell, _level.Map));
        }

        #endregion

        #region Behaviour

        protected override Cell CalculateTargetCell()
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

        private Cell GetUpCell()
        {
            return _level.Map[_pacman.GetX(), _pacman.GetY() - _distance];
        }

        private Cell GetDownCell()
        {
            return _level.Map[_pacman.GetX(), _pacman.GetY() + _distance];
        }

        private Cell GetLeftCell()
        {
            return _level.Map[_pacman.GetX() - _distance, _pacman.GetY()];
        }

        private Cell GetRightCell()
        {
            return _level.Map[_pacman.GetX() + _distance, _pacman.GetY()];
        }

        #endregion

        #region Movement constraints

        private bool IsUpAvailable()
        {
            return _pacman.GetY() - _distance > 0;
        }

        private bool IsDownAvailable()
        {
            return _pacman.GetY() + _distance < _level.Height - 1;
        }

        private bool IsLeftAvailable()
        {
            return _pacman.GetX() - _distance > 0;
        }

        private bool IsRightAvailable()
        {
            return _pacman.GetX() + _distance < _level.Width - 1;
        }

        #endregion
    }
}
