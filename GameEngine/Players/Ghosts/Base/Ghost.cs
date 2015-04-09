using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public enum Behaviour { Patrol, Chase, Frightened, Return }

    public abstract class Ghost : GameCharacter
    {
        #region Fields

        private const int runPathLength = 5;
        protected const int chasePathLength = 3;

        private const float chaseTime = 20.0f;
        private const float patrolTime = 7.0f;
        private const float returnTime = 7.0f;

        private float _patrolTime;
        private float _chaseTime;
        private float _returnTime;

        private List<Cell> _patrolPath;
        private List<Cell> _runPath;
        protected List<Cell> _chasePath;
        private List<Cell> _returnPath;

        protected int _pathIterator;
        private Cell _homeCell;
        private Cell _startCell;
        protected Cell _targetCell;

        private Behaviour _behaviour;

        private bool _isChanging;

        protected string _name;

        #endregion

        #region Properties

        public float PatrolTime
        {
            get
            {
                return _patrolTime;
            }
            set
            {
                _patrolTime = value;
            }
        }

        public float ChaseTime
        {
            get
            {
                return _chaseTime;
            }
            set
            {
                _chaseTime = value;
            }
        }

        public float ReturnTime
        {
            get
            {
                return _returnTime;
            }
            set
            {
                _returnTime = value;
            }
        }

        public List<Cell> PatrolPath
        {
            get
            {
                return _patrolPath;
            }
            set
            {
                _patrolPath = value;
            }
        }

        public List<Cell> RunPath
        {
            get
            {
                return _runPath;
            }
            set
            {
                _runPath = value;
            }
        }

        public List<Cell> ChasePath
        {
            get
            {
                return _chasePath;
            }
            set
            {
                _chasePath = value;
            }
        }

        public List<Cell> ReturnPath
        {
            get
            {
                return _returnPath;
            }
            set
            {
                _returnPath = value;
            }
        }

        public int PathIterator
        {
            get
            {
                return _pathIterator;
            }
            set
            {
                _pathIterator = value;
            }
        }

        public Cell HomeCell
        {
            get
            {
                return _homeCell;
            }
            set
            {
                _homeCell = value;
            }
        }

        public Cell StartCell
        {
            get
            {
                return _startCell;
            }
            set
            {
                _startCell = value;
            }
        }

        public Cell TargetCell
        {
            get
            {
                return _targetCell;
            }
            set
            {
                _targetCell = value;
            }
        }

        public Behaviour Behaviour
        {
            get
            {
                return _behaviour;
            }
            set
            {
                _behaviour = value;
            }
        }

        public bool IsChanging
        {
            get
            {
                return _isChanging;
            }
            set
            {
                _isChanging = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        #endregion

        #region Initialize

        public Ghost(Grid grid, float size)
            : base(grid, size)
        {
            _pathIterator = 0;
            _behaviour = Behaviour.Patrol;
            _patrolTime = patrolTime;
            _chaseTime = chaseTime;
            _returnTime = returnTime;
            _chasePath = new List<Cell>();
        }

        public Ghost(Grid grid, int x, int y, float size)
            : this(grid, size)
        {
            _otherSpeed = _fullSpeed / 2.0f;
            SetX(x);
            SetY(y);
            _homeCell = _level.Map[x - 1, y - 1];
        }

        public abstract void InitializePatrolPath();

        #endregion

        #region Movement

        public void Patrol()
        {
            FollowPath(_patrolPath);

            if (_pathIterator == _patrolPath.Count - 1)
            {
                _patrolPath.Reverse();
                _pathIterator = 0;
            }
        }

        public void Run()
        {
            FollowPath(_runPath);
        }

        public void Chase()
        {
            FollowPath(_chasePath);
        }

        public void ReturnHome()
        {
            FollowPath(_returnPath);
        }

        public void FollowPath(List<Cell> path)
        {
            if (_pathIterator < path.Count && _pathIterator > -1)
            {
                if (MoveTo(path.ElementAt(_pathIterator)))
                {
                    _pathIterator++;
                }
            }
        }

        public bool MoveTo(Cell cell)
        {
            TrySlowerSpeed();
            UpdateDirection(cell);

            if (GetBoundingRect() != cell.GetBoundingRect())
            {
                this.Move();
                return false;
            }
            else
            {
                _direction = Direction.None;
                _speed = _fullSpeed;
                return true;
            }
        }

        private void TrySlowerSpeed()
        {
            if (_behaviour == Behaviour.Frightened)
            {
                _speed = _otherSpeed;
            }
        }

        #endregion

        #region Update direction

        public void UpdateDirection(Cell other)
        {
            TryDirectionUp(other);

            TryDirectionDown(other);

            TryDirectionLeft(other);

            TryDirectionRight(other);
        }

        private void TryDirectionUp(Cell cell)
        {
            if (cell.GetBoundingRect().Top < this.GetBoundingRect().Top)
            {
                _direction = Direction.Up;
            }
        }

        private void TryDirectionDown(Cell cell)
        {
            if (cell.GetBoundingRect().Top > this.GetBoundingRect().Top)
            {
                _direction = Direction.Down;
            }
        }

        private void TryDirectionLeft(Cell cell)
        {
            if (cell.GetBoundingRect().Left < this.GetBoundingRect().Left)
            {
                _direction = Direction.Left;
            }
        }

        private void TryDirectionRight(Cell cell)
        {
            if (cell.GetBoundingRect().Left > this.GetBoundingRect().Left)
            {
                _direction = Direction.Right;
            }
        }

        #endregion

        #region Behaviour

        public void DoPatroling()
        {
            if (PatrolTime > 0)
            {
                Patrol();
            }
            else
            {
                _targetCell = CurrentCell();
                _chaseTime = chaseTime;
                _behaviour = Behaviour.Chase;
            }
        }

        public void UpdateRunPath(Cell randomCell)
        {
            List<Cell> bestPath = AStarAlgorithm.CalculatePath(CurrentCell(), randomCell, _level.Map);
            PathIterator = 0;

            if (bestPath.Count >= runPathLength)
            {
                _runPath = bestPath.GetRange(0, runPathLength);
            }
            else
            {
                _runPath = bestPath;
            }

            _targetCell = _runPath.Last();
        }

        public abstract void UpdateChasePath(Player pacman);

        public void DoChasing()
        {
            if (_chaseTime > 0)
            {
                Chase();
            }
            else
            {
                _returnPath = AStarAlgorithm.CalculatePath(CurrentCell(), _startCell, _level.Map);
                _behaviour = Behaviour.Return;
            }
        }

        // refactor
        public void DoReturning()
        {
            if (_returnTime > 0)
            {
                ReturnHome();
            }
            else
            {
                if (CurrentCell().GetBoundingRect() != _startCell.GetBoundingRect())
                {
                    _targetCell = CurrentCell();
                    _chaseTime = chaseTime;
                    _behaviour = Behaviour.Chase;
                }
            }

            if (CurrentCell().GetBoundingRect() == _startCell.GetBoundingRect())
            {
                _pathIterator = 0;
                _patrolTime = patrolTime;
                _behaviour = Behaviour.Patrol;
            }
        }

        #endregion

        #region Object methods

        public override string ToString()
        {
            return _name;
        }

        #endregion
    }
}
