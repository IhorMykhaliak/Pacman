using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Pacman.GameEngine;

namespace Pacman.ConsoleUI
{
    class EntryPoint
    {
        private static Game _game = new Game();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            _game = new Game();
            GameSubscribe();

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();

            while (pressedKey.Key != ConsoleKey.Escape)
            {
                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true);
                }

                _game.Player.PreviousDirection = _game.Player.Direction;
                _game.Player.PendingDirection = Direction.None;

                switch (pressedKey.Key)
                {
                    case ConsoleKey.UpArrow: _game.Player.Direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow: _game.Player.Direction = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow: _game.Player.Direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow: _game.Player.Direction = Direction.Right;
                        break;
                    case ConsoleKey.Spacebar: _game.PauseGame();
                        break;
                    case ConsoleKey.R: Restart();
                        break;
                    case ConsoleKey.Escape:
                        break;
                    default: _game.Player.Direction = _game.Player.PreviousDirection;
                        break;
                }

                pressedKey = new ConsoleKeyInfo();

                Thread.Sleep(100);

                Refresh();
            }

            Console.Clear();
            Console.WriteLine("Score: {0}", _game.Player.Coins * 10);
        }

        #region Game actions

        private static void Restart()
        {
            if (!_game.IsPaused)
            {
                GameUnsubscribe();
                _game.Dispose();
                _game = new Game();
                GameSubscribe();
                
                _game.IsPaused = false;
                _game.MainTimer.Enabled = true;
                Refresh();
            }
        }

        private static void Refresh()
        {
            if (!_game.IsPaused)
            {
                Console.Clear();
                Draw();
            }
        }

        private static void Draw()
        {
            Drawing.DrawGame(_game);
        }

        private static void PlayerWin()
        {
            _game.PauseGame();
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("You won !");
        }

        private static void PlayerDie()
        {
            _game.PauseGame();
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("You died !");
        }

        #endregion

        private static void GameSubscribe()
        {
            _game.Win += PlayerWin;
            _game.Die += PlayerDie;
        }

        private static void GameUnsubscribe()
        {
            _game.Win -= PlayerWin;
            _game.Die -= PlayerDie;
        }
    }
}
