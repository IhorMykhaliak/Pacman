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
        private float _deltaTime;

        private int _score;

        private Player _pacman;
        private List<Ghost> _ghosts;

        private char[,] _map;
        private Grid _level;

        private Timer _mainTimer;

        private GameLogic _gameLogic;

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
            set
            {
                _mainTimer = value;
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

        public GameLogic Logic
        {
            get
            {
                return _gameLogic;
            }
        }

        #endregion

        #region Initialization

        private void InitializeLevel()
        {
            string levelStruct = Pacman.GameEngine.Properties.Resources.mainLevel;
            string[] lines = levelStruct.Replace("\r\n", " ").Split(new char[] {' '});
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
            _ghosts.Add(new Blinky(_pacman, _level, 18, 15, size));
            _ghosts.Add(new Pinky(_pacman, _level, 17, 15, size));
            _ghosts.Add(new Inky(_pacman, _level, 18, 14, size));
            _ghosts.Add(new Clyde(_pacman, _level, 17, 14, size));

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

        private void InitializeLogic()
        {
            _deltaTime = ((float)(_mainTimer.Interval) / 1000.0f);

            _gameLogic = new GameLogic(_pacman, _ghosts, _level, _deltaTime);
            _gameLogic.PacmanDied += PacmanDie;
            _gameLogic.GhostDied += GhostDie;
            _gameLogic.PlayerWin += PlayerWin;
        }

        private void InitializeGame()
        {
            _isPaused = true;
            _elapsedTime = 0.0f;
            _pacmanCoins = _pacman.Coins;
            _score = 0;
        }

        public Game()
        {
            InitializeLevel();

            InitializePacman();

            InitializeGhosts();

            InitializeTimer();

            InitializeLogic();

            InitializeGame();
        }

        #endregion

        #region Events

        public event Action Update;

        public event Action Pause;

        public event Action Win;

        public event Action Die;

        #endregion

        #region Event handlers

        public void MainTimerTick(object sender, EventArgs e)
        {
            // Total time spent
            _elapsedTime += _deltaTime * 1000;

            // Pacman update
            _pacman.Move();
            _pacman.PickItem(_ghosts);
            _gameLogic.PowerUpCheck();

            // Ghosts update
            foreach (Ghost ghost in _ghosts)
            {
                _gameLogic.GhostCollisionCheck(ghost);
                _gameLogic.GhostBehaviourCheck(ghost);
            }

            _gameLogic.PacmanWinCheck();

            UpdateGame();
        }

        public void UpdateGame()
        {
            if (Update != null)
            {
                Update();
            }
        }

        public void PauseGame()
        {
            _mainTimer.Enabled = _isPaused;
            _isPaused = !_isPaused;
            if (Pause != null)
            {
                Pause();
            }
        }

        public void PlayerWin()
        {
            if (Win != null)
            {
                Win();
            }
        }

        private void PacmanDie()
        {
            _mainTimer.Stop();
            if (Die != null)
            {
                Die();
            }
        }

        private void GhostDie(Ghost ghost)
        {
            _score += 200;
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            _isPaused = false;
            _elapsedTime = 0.0f;
            _pacmanCoins = 0;
            _map = null;

            _mainTimer.Elapsed -= MainTimerTick;
            _mainTimer = null;

            _ghosts = null;
            _level = null;
        }

        #endregion
    }
}
