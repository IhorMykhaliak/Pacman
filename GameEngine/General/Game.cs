using System;
using System.Collections.Generic;
using System.Timers;

namespace Pacman.GameEngine
{
    public class Game : IDisposable
    {
        // Review IM : можлво варто описати карту як окремий клас Map, адже кожен ігровий об'єкт має
        //             кілька наперед визначених клітинок на карті, на які в певний момент гри посилається 
        //             ігрова логіка. Це б дозволило змінити, наприклад, _level.Map[8, 27] на Map.GhostHomeCell і тд...
        //             Таким чином можна уникнути захардкодженого доступу до конкретних у всьому коді.
        //             Також при створенні нових карт(в перспективі) не довелося б змінювати код.
        //             Тобто текстовий файл поряд з картою у собі б також містив певні клітинки для пакмана та привидів, тунелі тощо 
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
                ghost.ReturnPath = ghost.Algorithm.CalculatePath(ghost.CurrentCell(), ghost.StartCell, _level.Map);
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
            _mainTimer.Elapsed += new ElapsedEventHandler(OnMainTimerTick);
        }

        private void InitializeLogic()
        {
            _deltaTime = ((float)(_mainTimer.Interval) / 1000.0f);

            _gameLogic = new GameLogic(_pacman, _ghosts, _level, _deltaTime);
            _gameLogic.PacmanDied += OnPacmanDie;
            _gameLogic.GhostDied += OnGhostDie;
            _gameLogic.PlayerWin += OnPlayerWin;
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

        public event EventHandler<EventArgs> Update;

        public event EventHandler<EventArgs> Pause;

        public event EventHandler<EventArgs> Win;

        public event EventHandler<EventArgs> Die;

        public event EventHandler<EventArgs> NewGame;

        public event EventHandler<EventArgs> GhostDie;

        #endregion

        #region Event handlers

        public void OnMainTimerTick(object sender, EventArgs e)
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

            OnUpdateGame();
        }

        public void OnUpdateGame()
        {
            if (Update != null)
            {
                Update(this, EventArgs.Empty);
            }
        }

        public void OnPauseGame()
        {
            OnNewGame(); 

            _mainTimer.Enabled = _isPaused;
            _isPaused = !_isPaused;

            if (Pause != null)
            {
                Pause(this, EventArgs.Empty);
            }
        }

        public void OnPlayerWin(object sender, EventArgs e)
        {
            if (Win != null)
            {
                Win(this, EventArgs.Empty);
            }
        }

        public void OnNewGame()
        {
            if (NewGame != null)
            {
                NewGame(this, EventArgs.Empty);
            }
        }

        private void OnPacmanDie(object sender, EventArgs e)
        {
            _mainTimer.Stop();
            if (Die != null)
            {
                Die(this, EventArgs.Empty);
            }
        }

        private void OnGhostDie(object sender, Pacman.GameEngine.GameLogic.GhostEventArgs e)
        {
            #region Validation

            if (e.Ghost == null)
            {
                throw new ArgumentNullException();
            }

            #endregion

            if (GhostDie != null)
            {
                GhostDie(this, EventArgs.Empty);
            }
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

            _mainTimer.Elapsed -= OnMainTimerTick;
            _mainTimer = null;

            _ghosts = null;
            _level = null;
        }

        #endregion
    }
}
