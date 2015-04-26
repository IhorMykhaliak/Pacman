using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public enum Content { Wall, Coin, PowerUp, Empty }

    public class Cell : IGameObject
    {
        #region Fields

        private float _xPos;
        private float _yPos;
        private float _size;

        private Content _content;

        private Cell _parent;
        private int _manhattanHeuristics;

        #endregion

        #region Properties

        public float Size
        {
            get
            {
                return _size;
            }
        }

        public Content Content
        {
            get
            {
                return _content;
            }
            internal set
            {
                _content = value;
            }
        }

        public Cell Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public int ManhattanHeuristics
        {
            get
            {
                return _manhattanHeuristics;
            }
            set
            {
                _manhattanHeuristics = value;
            }
        }

        #endregion

        #region Position

        public int GetX()
        {
            return (int)(_xPos / _size) - 1;
        }

        public int GetY()
        {
            return (int)(_yPos / _size) - 1;
        }

        public BoundingSquare GetBoundingRect()
        {
            return new BoundingSquare(_xPos - _size, _yPos - _size, _size);
        }

        #endregion

        #region Initialization

        public Cell()
        {
            _xPos = 0.0f;
            _yPos = 0.0f;
            _content = Content.Empty;
        }

        public Cell(float x, float y)
            : this()
        {
            _xPos = x;
            _yPos = y;
        }

        public Cell(float x, float y, Content c, float size)
            : this(x, y)
        {
            #region Validation

            if (size <= 0)
            {
                throw new ArgumentException("Cell size has to be greater than 0");
            }

            #endregion

            _content = c;
            _size = size;
        }

        #endregion

        #region Check conditions

        public bool IsWall()
        {
            return _content == Content.Wall;
        }

        public bool IsCoin()
        {
            return _content == Content.Coin;
        }

        public bool IsPowerUp()
        {
            return _content == Content.PowerUp;
        }

        public bool IsEmpty()
        {
            return _content == Content.Empty;
        }

        public bool IsNextTo(Cell other)
        {
            #region Validation

            if (other == null)
            {
                throw new ArgumentNullException("Cell invalid");
            }

            #endregion

            int xDiff = Math.Abs(this.GetX() - other.GetX());
            int yDiff = Math.Abs(this.GetY() - other.GetY());

            return xDiff + yDiff < 2 && xDiff + yDiff != 0;
        }

        #endregion

        #region Object methods

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", GetX(), GetY());
        }

        #endregion
    }
}
