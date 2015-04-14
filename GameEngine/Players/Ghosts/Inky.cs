using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public class Inky : Ghost
    {
        #region Fields

        private int _xShift;
        private int _yShift;

        private int _xDistance;
        private int _yDistance;

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
            _distance = 2;
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[21, 27], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[21, 27], _level.Map[23, 30], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[23, 30], StartCell, _level.Map));
        }

        #endregion

        #region Behaviour

        public override Cell CalculateTargetCell()
        {
            Cell offsetCell = CalculateOffsetCell();
            Cell cell;

            _xDistance = _pacman.GetX() - _blinky.GetX();
            _yDistance = _pacman.GetY() - _blinky.GetY();

            TargetInBounds();

            cell = _level.Map[_pacman.GetX() + _xDistance, _pacman.GetY() + _yDistance];

            return TargetEmpty(cell);
        }

        private Cell CalculateOffsetCell()
        {
            Cell offsetCell = new Cell();
            offsetCell.Content = Content.Wall;

            while (offsetCell.IsWall())
            {
                switch (_pacman.Direction)
                {
                    case Direction.Up: offsetCell = GetUpCell();
                        break;
                    case Direction.Down: offsetCell = GetDownCell();
                        break;
                    case Direction.Left:
                        {
                            if (IsLeftAvailable())
                            {
                                offsetCell = GetLeftCell();
                            }
                            break;
                        }
                    case Direction.Right:
                        {
                            if (IsRightAvailable())
                            {
                                offsetCell = GetRightCell();
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

        private void TargetInBounds()
        {
            LeftBoundShift();

            RightBoundShift();

            DownBoundShift();

            UpBoundShift();
        }

        private void LeftBoundShift()
        {
            while (_pacman.GetX() + _xDistance < 0)
            {
                _xDistance++;
            }
        }

        private void RightBoundShift()
        {
            while (_pacman.GetX() + _xDistance > _level.Width - 1)
            {
                _xDistance--;
            }
        }

        private void DownBoundShift()
        {
            while (_pacman.GetY() + _yDistance < 0)
            {
                _yDistance++;
            }
        }

        private void UpBoundShift()
        {
            while (_pacman.GetY() + _yDistance > _level.Height - 1)
            {
                _yDistance--;
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
                upperBound = CalculateUpperBound(count);

                for (; _yShift <= upperBound; _yShift++)
                {
                    if (IsShiftedInBounds(cell))
                    {
                        ShiftCell(ref cell);
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

        private int CalculateUpperBound(int count)
        {
            int bound;

            if (count % 2 == 0)
            {
                _xShift = count / 2;
                _yShift = -_xShift;
                bound = _xShift;
            }
            else
            {
                _xShift = -count / 2;
                bound = -_xShift;
                _yShift = _xShift;
            }

            return bound;
        }

        private void ShiftCell(ref Cell cell)
        {
            cell = _level.Map[cell.GetX() + _xShift, cell.GetY() + _yShift];
        }

        #endregion
    }
}
