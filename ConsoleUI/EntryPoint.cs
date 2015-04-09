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

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();

            while (pressedKey.Key != ConsoleKey.Escape)
            {
                pressedKey = Console.ReadKey(true);

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
                    case ConsoleKey.Spacebar: _game.Pause();
                        break;
                    case ConsoleKey.R: Restart();
                        break;
                    case ConsoleKey.Escape:
                        break;
                    default: _game.Player.Direction = _game.Player.PreviousDirection;
                        break;
                }

                Refresh();
                WinCheck();
            }

            Console.Clear();
            Console.WriteLine("Score: {0}", _game.Player.Coins * 10);
        }

        private static void Restart()
        {
            if (!_game.IsPaused)
            {
                _game.Dispose();
                _game = new Game();
                _game.IsPaused = false;
                _game.MainTimer.Enabled = true;
                Refresh();
            }
        }

        private static void Refresh()
        {
            Console.Clear();
            if (!_game.IsPaused)
            {
                Draw();
            }
        }

        private static void Draw()
        {
            Drawing.DrawLevel(_game.Level);
            Drawing.DrawPacman(_game.Player);
            foreach (Ghost ghost in _game.Ghosts)
            {
                Drawing.DrawGhost(ghost);
            }
        }

        private static void WinCheck()
        {
            if (_game.Player.Coins == _game.Level.Coins)
            {
                Console.WriteLine("You won !");
            }
        }
    }
}
