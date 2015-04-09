using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pacman.GameEngine;

namespace Pacman.ConsoleUI
{
    class Drawing
    {
        public static void DrawCell(Cell cell)
        {
            DrawContent(cell);
        }

        public static void DrawContent(Cell cell)
        {
            switch (cell.Content)
            {
                case Content.Wall: DrawWall(cell);
                    break;
                case Content.Coin: DrawCoin(cell);
                    break;
                case Content.PowerUp: DrawPowerUp(cell);
                    break;
                case Content.Empty: break;
                default: break;
            }
        }

        public static void DrawWall(Cell cell)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(cell.GetX(), cell.GetY());
            Console.Write('█');
        }

        public static void DrawCoin(Cell cell)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(cell.GetX(), cell.GetY());
            Console.Write('o');
        }

        public static void DrawPowerUp(Cell cell)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(cell.GetX(), cell.GetY());
            Console.Write('p');
        }

        public static void DrawPacman(Player pacman)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition((int)((pacman.GetLeft() + pacman.Size / 2) / (pacman.Size)),
                                      (int)((pacman.GetBottom() - pacman.Size / 2) / (pacman.Size)));
            Console.Write('*');
        }

        public static void DrawGhost(Ghost ghost)
        {
            ConsoleColor color;
            switch (ghost.Behaviour)
            {
                case Behaviour.Patrol: color = ConsoleColor.Red;
                    break;
                case Behaviour.Frightened: color = ConsoleColor.Magenta;
                    break;
                case Behaviour.Chase: color = ConsoleColor.Cyan;
                    break;
                default: color = ConsoleColor.Red;
                    break;
            }

            Console.SetCursorPosition((int)((ghost.GetLeft() + ghost.Size / 2) / (ghost.Size)),
                                      (int)((ghost.GetBottom() - ghost.Size / 2) / (ghost.Size)));
            Console.ForegroundColor = color;
            Console.Write('*');
        }

        public static void DrawLevel(Grid grid)
        {
            for (int i = 0; i < grid.Width; i++)
            {
                for (int j = 0; j < grid.Height; j++)
                {
                    DrawCell(grid.Map[i, j]);
                }
            }
        }
    }
}
