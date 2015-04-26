using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public enum Direction { Up, Down, Left, Right, None }

    public abstract class GameCharacter : IGameObject
    {
        #region Protected fields

        protected const float _epsilon = 0.001f;

        protected float _xPos;
        protected float _yPos;
        protected float _size;
        protected float _speed;
        protected float _fullSpeed;
        protected float _otherSpeed;

        protected bool _isPassedRightTunnel;
        protected bool _isPassedLeftTunnel;

        protected Grid _level;

        protected Direction _direction;
        protected Direction _previousDirection;
        protected Direction _pendingDirection;

        #endregion

        #region Properies

        public float Size
        {
            get
            {
                return _size;
            }
        }

        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
            }
        }

        public Direction PreviousDirection
        {
            get
            {
                return _previousDirection;
            }
            set
            {
                _previousDirection = value;
            }
        }

        public Direction PendingDirection
        {
            get
            {
                return _pendingDirection;
            }
            set
            {
                _pendingDirection = value;
            }
        }

        public bool IsPassedRightTunnel
        {
            get
            {
                return _isPassedRightTunnel;
            }
            set
            {
                _isPassedRightTunnel = value;
            }
        }

        public bool IsPassedLeftTunnel
        {
            get
            {
                return _isPassedLeftTunnel;
            }
            set
            {
                _isPassedLeftTunnel = value;
            }
        }

        #endregion

        #region Position

        public float GetTop()
        {
            return _yPos - _size;
        }

        public float GetBottom()
        {
            return _yPos;
        }

        public float GetLeft()
        {
            return _xPos - _size;
        }

        public float GetRight()
        {
            return _xPos;
        }

        public int GetX()
        {
            return (int)((_xPos - _size / 2) / _size);
        }

        public int GetY()
        {
            return (int)((_yPos - _size / 2) / _size);
        }

        public BoundingSquare GetBoundingRect()
        {
            return new BoundingSquare(_xPos - _size, _yPos - _size, _size);
        }

        public Cell CurrentCell()
        {
            return _level.Map[GetX(), GetY()];
        }

        #endregion

        #region Initializtion

        public void SetX(int x)
        {
            #region Validation

            if (x < 0)
            {
                throw new ArgumentException();
            }

            #endregion

            _xPos = x * _size;
        }

        public void SetY(int y)
        {
            #region Validation

            if (y < 0)
            {
                throw new ArgumentException();
            }

            #endregion

            _yPos = y * _size;
        }

        public GameCharacter()
        {
            SetX(1);
            SetY(1);

            _fullSpeed = 2.0f;
            _speed = _fullSpeed;

            _direction = Direction.None;
            _previousDirection = Direction.None;
            _pendingDirection = Direction.None;
        }

        public GameCharacter(Grid grid, float size)
            : this()
        {
            #region Validation

            if (grid == null)
            {
                throw new ArgumentNullException();
            }

            #endregion

            _level = grid;
            _size = size;
        }

        #endregion

        #region Movement

        public void Move()
        {
            if (CanMove(_pendingDirection))
            {
                _direction = _pendingDirection;
            }

            TryPassTunnel();

            switch (_direction)
            {
                case Direction.Up: MoveUp();
                    break;
                case Direction.Down: MoveDown();
                    break;
                case Direction.Left: MoveLeft();
                    break;
                case Direction.Right: MoveRight();
                    break;
                case Direction.None: break;
                default: break;
            }

            _previousDirection = Direction.None;
        }

        public virtual void TryPassTunnel()
        {
            TryPassLeftTunnel();
            TryPassRightTunnel();
        }

        public virtual void TryPassLeftTunnel()
        {
            if (GetX() == 0 && _direction == Direction.Left)
            {
                SetX(_level.Width - 1);
                _isPassedLeftTunnel = true;
                _isPassedRightTunnel = false;
            }
        }

        public virtual void TryPassRightTunnel()
        {
            if (GetX() == _level.Width - 1 && _direction == Direction.Right)
            {
                SetX(0);
                _isPassedRightTunnel = true;
                _isPassedLeftTunnel = false;
            }
        }

        private void MoveUp()
        {
            if (CanMoveUp())
            {
                _yPos -= _speed;
            }
            else
            {
                _direction = _previousDirection;
                _previousDirection = Direction.None;
                _pendingDirection = Direction.Up;
            }
        }

        private void MoveDown()
        {
            if (CanMoveDown())
            {
                _yPos += _speed;
            }
            else
            {
                _direction = _previousDirection;
                _previousDirection = Direction.None;
                _pendingDirection = Direction.Down;
            }
        }

        private void MoveLeft()
        {
            if (CanMoveLeft())
            {
                _xPos -= _speed;
            }
            else
            {
                _direction = _previousDirection;
                _previousDirection = Direction.None;
                _pendingDirection = Direction.Left;
            }
        }

        private void MoveRight()
        {
            if (CanMoveRight())
            {
                _xPos += _speed;
            }
            else
            {
                _direction = _previousDirection;
                _previousDirection = Direction.None;
                _pendingDirection = Direction.Right;
            }
        }

        #endregion

        #region Movement constraints

        protected bool CanMove(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return CanMoveUp();
                case Direction.Down: return CanMoveDown();
                case Direction.Left: return CanMoveLeft();
                case Direction.Right: return CanMoveRight();
                default: return false;
            }
        }

        protected bool CanMoveUp()
        {
            int xTemp1, xTemp2, yTemp;
            xTemp1 = (int)((GetLeft() + _epsilon) / _size);
            xTemp2 = (int)((GetRight() - _epsilon) / _size);
            yTemp = (int)((_yPos / _size) - _epsilon) - 1;

            if (xTemp1 >= 0 && xTemp1 < _level.Width &&
                yTemp >= 0 && yTemp < _level.Height &&
                xTemp2 >= 0 && xTemp2 < _level.Width)
            {
                Cell cell1 = _level.Map[xTemp1, yTemp];
                Cell cell2 = _level.Map[xTemp2, yTemp];
                return !cell1.IsWall() && !cell2.IsWall();
            }
            else
            {
                return true;
            }
        }

        protected bool CanMoveDown()
        {
            int xTemp1, xTemp2, yTemp;
            xTemp1 = (int)((GetLeft() + _epsilon) / _size);
            xTemp2 = (int)((GetRight() - _epsilon) / _size);
            yTemp = (int)((_yPos / _size) + _epsilon);

            if (xTemp1 >= 0 && xTemp1 < _level.Width &&
                yTemp >= 0 && yTemp < _level.Height &&
                xTemp2 >= 0 && xTemp2 < _level.Width)
            {
                Cell cell1 = _level.Map[xTemp1, yTemp];
                Cell cell2 = _level.Map[xTemp2, yTemp];
                return !cell1.IsWall() && !cell2.IsWall();
            }
            else
            {
                return true;
            }
        }

        protected bool CanMoveLeft()
        {
            int yTemp1, yTemp2, xTemp;
            yTemp1 = (int)((GetTop() + _epsilon) / _size);
            yTemp2 = (int)((GetBottom() - _epsilon) / _size);
            xTemp = (int)((_xPos / _size) - _epsilon);

            if (yTemp1 >= 0 && yTemp1 < _level.Height &&
                xTemp > 0 && xTemp <= _level.Width &&
                yTemp2 >= 0 && yTemp2 < _level.Height)
            {
                Cell cell1 = _level.Map[xTemp - 1, yTemp1];
                Cell cell2 = _level.Map[xTemp - 1, yTemp2];
                return !cell1.IsWall() && !cell2.IsWall();
            }
            else
            {
                return true;
            }
        }

        protected bool CanMoveRight()
        {
            int yTemp1, yTemp2, xTemp;
            yTemp1 = (int)((GetTop() + _epsilon) / _size);
            yTemp2 = (int)((GetBottom() - _epsilon) / _size);
            xTemp = (int)((_xPos / _size) + _epsilon);

            if (yTemp1 >= 0 && yTemp1 < _level.Height &&
                xTemp >= 0 && xTemp < _level.Width &&
                yTemp2 >= 0 && yTemp2 < _level.Height)
            {
                Cell cell1 = _level.Map[xTemp, yTemp1];
                Cell cell2 = _level.Map[xTemp, yTemp2];
                return !cell1.IsWall() && !cell2.IsWall();
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
