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
            try
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
                        case ConsoleKey.R: Restart(null, EventArgs.Empty);
                            break;
                        case ConsoleKey.Escape:
                            break;
                        default: _game.Player.Direction = _game.Player.PreviousDirection;
                            break;
                    }

                    pressedKey = new ConsoleKeyInfo();

                    Thread.Sleep(100);

                    Refresh(null, EventArgs.Empty);
                }
            }
            catch (Exception exc)
            {
                Console.Clear();
                Console.WriteLine(exc.Message);
            }
        }

        #region Game actions

        private static void Restart(object sender, EventArgs e)
        {
            GameUnsubscribe();
            _game.Dispose();
            _game = new Game();
            GameSubscribe();
            
            _game.IsPaused = false;
            _game.MainTimer.Enabled = true;
            Refresh(null, EventArgs.Empty);
        }

        private static void Refresh(object sender, EventArgs e)
        {
            if (!_game.IsPaused)
            {
                Console.Clear();
                Draw(null, EventArgs.Empty);
            }
            else
            {
                DisplayMenu();
            }
        }

        private static void Draw(object sender, EventArgs e)
        {
            Drawing.DrawGame(_game);
        }

        private static void PlayerWin(object sender, EventArgs e)
        {
            _game.PauseGame();
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("You won !");
        }

        private static void PlayerDie(object sender, EventArgs e)
        {
            _game.PauseGame();
            Console.Clear();
            Console.SetCursorPosition(0, 0);
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

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("PLEASE OPEN IN BIG CONSOLE WINDOW !!!");
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Press -Space- to play/pause.");
            Console.WriteLine("Use -arrow keys- to move pacman.");
            Console.WriteLine("If you die or just want to try it out press -R- to restart.");
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Eat all green -o-'s to win.");
            Console.WriteLine("Eat purple -p-'s to power up for limited time, so you can eat enemies.");
            Console.WriteLine("If you are not powered up enemis kill you in one touch.");
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine();
            Console.WriteLine("Score: {0}", _game.Score);
        }
    }
}
