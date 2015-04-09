using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace Pacman.GameEngine
{
    public class Game : IDisposable
    {
        #region Fields

        private const float size = 20;

        private bool _isPaused;
        private float _elapsedTime;
        private float _pacmanCoins;
        private readonly float _deltaTime;

        private int _score;

        private Player _pacman;
        private List<Ghost> _ghosts;

        private char[,] _map;
        private Grid _level;

        private Timer _mainTimer;

        #endregion

        #region Properties

        public Player Player
        {
            get
            {
                return _pacman;
            }
        }

        public float ElapsedSeconds
        {
            get
            {
                return _elapsedTime / 1000.0f;
            }
        }

        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;
            }
        }

        public int Score
        {
            get
            {
                return _score + _pacman.Coins * 10;
            }
        }

        public Timer MainTimer
        {
            get
            {
                return _mainTimer;
            }
        }

        public List<Ghost> Ghosts
        {
            get
            {
                return _ghosts;
            }
        }

        public Grid Level
        {
            get
            {
                return _level;
            }
        }

        #endregion

        #region Initialization

        private void InitializeLevel()
        {
            //string[] lines = GameEngine.Properties.Resources.mainLevel
            string[] lines = File.ReadAllLines(@"mainLevel.txt");
            _map = new char[lines.Length, lines[0].Length];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    _map[i, j] = lines[i][j];
                }
            }

            _level = new Grid(_map, size);
        }

        private void InitializePacman()
        {
            _pacman = new Player(_level, 17, 25, size);
        }

        private void InitializeGhosts()
        {
            _ghosts = new List<Ghost>();
            _ghosts.Add(new Blinky(_level, 18, 15, size));
            _ghosts.Add(new Pinky(_level, 17, 15, size));
            _ghosts.Add(new Inky(_level, 18, 14, size));
            _ghosts.Add(new Clyde(_level, 17, 14, size));

            _ghosts[0].StartCell = _level.Map[27, 2];
            _ghosts[1].StartCell = _level.Map[6, 2];
            _ghosts[2].StartCell = _level.Map[27, 27];
            _ghosts[3].StartCell = _level.Map[4, 27];

            foreach (Ghost ghost in _ghosts)
            {
                ghost.ReturnPath = AStarAlgorithm.CalculatePath(ghost.CurrentCell(), ghost.StartCell, _level.Map);
                ghost.Behaviour = Behaviour.Return;
                ghost.InitializePatrolPath();
            }

            ((Inky)_ghosts[2]).Blinky = (Blinky)_ghosts[0];
        }

        private void InitializeTimer()
        {
            _mainTimer = new Timer();
            _mainTimer.Interval = 10;
            _mainTimer.Enabled = false;
            _mainTimer.Elapsed += new ElapsedEventHandler(MainTimerTick);
        }

        public Game()
        {
            InitializeLevel();

            InitializePacman();

            InitializeGhosts();

            InitializeTimer();

            _deltaTime = ((float)(_mainTimer.Interval) / 1000.0f);

            _isPaused = true;
            _elapsedTime = 0.0f;
            _pacmanCoins = _pacman.Coins;
            _score = 0;
        }

        #endregion

        #region Events

        public event Action Update;

        #endregion

        #region Event handlers

        public void MainTimerTick(object sender, EventArgs e)
        {
            // Total time spent
            _elapsedTime += _deltaTime * 1000;

            // Pacman update
            _pacman.Move();
            _pacman.PickItem(_ghosts);
            PowerUpCheck();

            // Ghost update
            foreach (Ghost ghost in _ghosts)
            {
                GhostCollisionCheck(ghost);
                GhostBehaviourCheck(ghost);
            }

            UpdateGame();
        }

        public void UpdateGame()
        {
            if (Update != null)
            {
                Update();
            }
        }

        public void Pause()
        {
            _mainTimer.Enabled = _isPaused;
            _isPaused = !_isPaused;
        }

        #endregion

        #region Pacman behaviour

        // refactor this
        private void PowerUpCheck()
        {
            if (_pacman.IsPoweredUp)
            {
                _pacman.PowerUpTime -= _deltaTime;
                if (_pacman.PowerUpTime > 0)
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
                else
                {
                    PowerDown();
                }
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

        // implement pacman die
        private void PacmanDie()
        {
            _mainTimer.Stop();
            Console.WriteLine("Game over!");
        }

        #endregion

        #region Ghosts behaviour

        private void GhostDie(Ghost ghost)
        {
            ghost.SetX(ghost.HomeCell.GetX() + 1);
            ghost.SetY(ghost.HomeCell.GetY() + 1);
            ghost.Direction = Direction.None;
            ghost.TargetCell = ghost.CurrentCell();
            _score += 200;
        }

        // change message box
        private void GhostCollisionCheck(Ghost ghost)
        {
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

        private void GhostBehaviourCheck(Ghost ghost)
        {
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
                ghost.UpdateRunPath(_level.GetRandomEmptyCell());
            }

            ghost.Run();
        }

        private void GhostChasing(Ghost ghost)
        {
            if (ghost.TargetCell == ghost.CurrentCell())
            {
                ghost.UpdateChasePath(_pacman);
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

        #region IDisposable members

        public void Dispose()
        {
            _isPaused = false;
            _elapsedTime = 0.0f;
            _pacmanCoins = 0;

            _pacman = null;
            _ghosts = null;

            _map = null;
            _level = null;

            _mainTimer.Elapsed -= MainTimerTick;
            _mainTimer = null;
        }

        #endregion
    }
}
