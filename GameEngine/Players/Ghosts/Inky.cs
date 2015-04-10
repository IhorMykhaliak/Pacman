using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    class Inky : Ghost
    {
        #region Fields

        private int _xShift;
        private int _yShift;
        private int _distance = 2;
        private Blinky _blinky;

        #endregion

        #region Properties

        public Blinky Blinky
        {
            set
            {
                _blinky = value;
            }
        }

        #endregion

        #region Initialization

        public Inky(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            _name = "Inky";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[21, 27], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[21, 27], _level.Map[23, 30], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[23, 30], StartCell, _level.Map));
        }

        #endregion

        private Cell CalculateOffsetCell()
        {
            Cell offsetCell = new Cell();
            offsetCell.Content = Content.Wall;

            while (offsetCell.IsWall())
            {
                switch (_pacman.Direction)
                {
                    case Direction.Up: offsetCell = _level.Map[_pacman.GetX(), _pacman.GetY() - _distance];
                        break;
                    case Direction.Down: offsetCell = _level.Map[_pacman.GetX(), _pacman.GetY() + _distance];
                        break;
                    case Direction.Left:
                        {
                            if (_pacman.GetX() - _distance > 0)
                            {
                                offsetCell = _level.Map[_pacman.GetX() - _distance, _pacman.GetY()];
                            }
                            break;
                        }
                    case Direction.Right:
                        {
                            if (_pacman.GetX() + _distance < _level.Width - 1)
                            {
                                offsetCell = _level.Map[_pacman.GetX() + _distance, _pacman.GetY()];
                            }
                            break;
                        }
                    default: offsetCell = _pacman.CurrentCell();
                        break;
                }

                if (_distance == 0)
                {
                    offsetCell = _pacman.CurrentCell();
                    break;
                }

                _distance--;
            }

            return offsetCell;
        }

        protected override Cell CalculateTargetCell()
        {
            Cell offsetCell = CalculateOffsetCell();
            Cell cell;
            int xDistance, yDistance;

            xDistance = _pacman.GetX() - _blinky.GetX();
            yDistance = _pacman.GetY() - _blinky.GetY();

            TargetInBounds(ref xDistance, ref yDistance);

            cell = _level.Map[_pacman.GetX() + xDistance, _pacman.GetY() + yDistance];

            return TargetEmpty(cell);
        }

        private void TargetInBounds(ref int xDistance, ref int yDistance)
        {
            while (_pacman.GetX() + xDistance < 0)
            {
                xDistance++;
            }

            while (_pacman.GetX() + xDistance > _level.Width - 1)
            {
                xDistance--;
            }

            while (_pacman.GetY() + yDistance < 0)
            {
                yDistance++;
            }

            while (_pacman.GetY() + yDistance > _level.Height - 1)
            {
                yDistance--;
            }
        }

        private Cell TargetEmpty(Cell cell)
        {
            int upperBound;
            int count = 1;

            _xShift = 0;
            _yShift = 0;

            while (cell.IsWall())
            {
                if (count % 2 == 0)
                {
                    _xShift = count / 2;
                    _yShift = -_xShift;
                    upperBound = _xShift;
                }
                else
                {
                    _xShift = -count / 2;
                    upperBound = -_xShift;
                    _yShift = _xShift;
                }

                for (; _yShift <= upperBound; _yShift++)
                {
                    if (IsShiftedInBounds(cell))
                    {
                        cell = _level.Map[cell.GetX() + _xShift, cell.GetY() + _yShift];
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

        private bool IsShiftedInBounds(Cell cell)
        {
            return cell.GetX() + _xShift > 0 &&
                   cell.GetX() + _xShift < _level.Width &&
                   cell.GetY() + _yShift > 0 &&
                   cell.GetY() + _yShift < _level.Height;
        }
    }
}
