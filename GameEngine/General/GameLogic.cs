using System;
using System.Collections.Generic;
using System.Linq;


namespace Pacman.GameEngine
{
    public class GameLogic
    {
        #region Fields

        private Player _pacman;
        private List<Ghost> _ghosts;
        private Grid _level;
        private float _deltaTime;

        #endregion

        #region Initialization

        public GameLogic(Player pacman, List<Ghost> ghosts, Grid level, float deltaTime)
        {
            #region Validation

            if (deltaTime < 0.0f)
            {
                throw new ArgumentException("Delta time has to be > 0.0f");
            }

            if (pacman == null)
            {
                throw new ArgumentNullException();
            }

            if (level == null)
            {
                throw new ArgumentNullException();
            }

            if (ghosts == null)
            {
                throw new ArgumentNullException();
            }

            #endregion

            this._pacman = pacman;
            this._ghosts = ghosts;
            this._level = level;
            this._deltaTime = deltaTime;
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> PacmanDied;

        public event EventHandler<GhostEventArgs> GhostDied;

        public event EventHandler<EventArgs> PlayerWin;

        #endregion

        #region Pacman behaviour

        public void PowerUpCheck()
        {
            if (_pacman.IsPoweredUp)
            {
                _pacman.PowerUpTime -= _deltaTime;

                if (_pacman.PowerUpTime > 0)
                {
                    WhilePoweredUp();
                }
                else
                {
                    PowerDown();
                }
            }
        }

        public void PacmanWinCheck()
        {
            if (PlayerWin != null && IsPlayerWinner())
            {
                PlayerWin(this, EventArgs.Empty);
            }
        }

        private void WhilePoweredUp()
        {
            foreach (Ghost ghost in _ghosts)
            {
                if (_pacman.PowerUpTime > 1)
                {
                    ghost.IsChanging = false;
                }
                else
                    if (_pacman.PowerUpTime > 0)
                    {
                        ghost.IsChanging = true;
                    }

                ghost.Behaviour = Behaviour.Frightened;
            }
        }

        private void PowerDown()
        {
            _pacman.IsPoweredUp = false;
            foreach (Ghost ghost in _ghosts)
            {
                ghost.TargetCell = ghost.CurrentCell();
                ghost.Behaviour = Behaviour.Chase;
            }
        }

        private void PacmanDie()
        {
            if (PacmanDied != null)
            {
                PacmanDied(this, EventArgs.Empty);
            }
        }

        private bool IsPlayerWinner()
        {
            return _pacman.Coins == _level.Coins;
        }

        #endregion

        #region Ghosts behaviour

        public void GhostDie(Ghost ghost)
        {
            #region Validation

            if (ghost == null)
            {
                throw new ArgumentNullException();
            }

            #endregion

            ghost.SetX(ghost.HomeCell.GetX() + 1);
            ghost.SetY(ghost.HomeCell.GetY() + 1);
            ghost.Direction = Direction.None;
            ghost.TargetCell = ghost.CurrentCell();
            if (GhostDied != null)
            {
                GhostDied(this, new GhostEventArgs(ghost));
            }
        }

        public void GhostCollisionCheck(Ghost ghost)
        {
            #region Validation

            if (ghost == null)
            {
                throw new ArgumentNullException();
            }

            #endregion

            if (_pacman.CurrentCell() == ghost.CurrentCell())
            {
                if (ghost.Behaviour != Behaviour.Frightened)
                {
                    PacmanDie();
                }
                else
                {
                    GhostDie(ghost);
                }
            }
        }

        public void GhostBehaviourCheck(Ghost ghost)
        {
            #region Validation

            if (ghost == null)
            {
                throw new ArgumentNullException();
            }

            #endregion

            switch (ghost.Behaviour)
            {
                case Behaviour.Patrol: GhostPatroling(ghost);
                    break;
                case Behaviour.Frightened: GhostFrightened(ghost);
                    break;
                case Behaviour.Chase: GhostChasing(ghost);
                    break;
                case Behaviour.Return: GhostReturning(ghost);
                    break;
            }
        }

        private void GhostPatroling(Ghost ghost)
        {
            ghost.PatrolTime -= _deltaTime;
            ghost.DoPatroling();
        }

        private void GhostFrightened(Ghost ghost)
        {
            if (ghost.TargetCell == ghost.CurrentCell())
            {
                ghost.UpdateRunPath(_level.GetRandomFreeCell());
            }

            ghost.Run();
        }

        private void GhostChasing(Ghost ghost)
        {
            if (ghost.TargetCell == ghost.CurrentCell())
            {
                ghost.UpdateChasePath();
            }

            ghost.ChaseTime -= _deltaTime;
            ghost.DoChasing();
        }

        private void GhostReturning(Ghost ghost)
        {
            ghost.ReturnTime -= _deltaTime;
            ghost.DoReturning();
        }

        #endregion

        #region GhostEventArgs

        public class GhostEventArgs : EventArgs
        {
            public Ghost Ghost { get; set; }

            public GhostEventArgs(Ghost ghost)
            {
                Ghost = ghost;
            }
        }

        #endregion
    }
}
