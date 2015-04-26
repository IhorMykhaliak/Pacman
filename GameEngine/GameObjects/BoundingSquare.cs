using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public class BoundingSquare
    {
        #region Fields

        private float _x;
        private float _y;
        private float _size;

        #endregion

        #region Properties

        public float X
        {
            get
            {
                return this._x;
            }
        }

        public float Y
        {
            get
            {
                return this._y;
            }
        }

        public float Size
        {
            get
            {
                return this._size;
            }
        }

        public float Top
        {
            get
            {
                return this._y;
            }
        }

        public float Bottom
        {
            get
            {
                return this._y + this.Size;
            }
        }

        public float Left
        {
            get
            {
                return this._x;
            }
        }

        public float Right
        {
            get
            {
                return this._x + this._size;
            }
        }

        #endregion

        #region Initialization

        public BoundingSquare(float x, float y, float size)
        {
            #region Validation

            if (size <= 0)
            {
                throw new ArgumentException("Size has to be greater than 0");
            }

            #endregion

            this._x = x;
            this._y = y;
            this._size = size;
        }

        #endregion

        #region Operators

        public static bool operator ==(BoundingSquare square1, BoundingSquare square2)
        {
            return square1.X == square2.X && 
                   square1.Y == square2.Y && 
                   square1.Size == square2.Size;
        }

        public static bool operator !=(BoundingSquare square1, BoundingSquare square2)
        {
            return !(square1 == square2);
        }

        #endregion

        #region Check conditions

        public bool IntersectsWith(BoundingSquare other)
        {
            return IntersectsByX(other) && IntersectsByY(other);
        }

        private bool IntersectsByX(BoundingSquare other)
        {
            return (other.Right >= this.Left && other.Left <= this.Right);
        }

        private bool IntersectsByY(BoundingSquare other)
        {
            return (other.Bottom >= this.Top && other.Top <= this.Bottom);
        }

        #endregion

        #region Object methods

        public override bool Equals(object obj)
        {
            if (obj is BoundingSquare)
            {
                return this == (BoundingSquare)obj;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (int)this._x ^ (int)this._y;
        }

        #endregion
    }
}
