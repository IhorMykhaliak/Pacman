using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pacman.GameEngine;
using System.Drawing;
using System.Windows.Forms;

namespace Pacman.DesktopUI
{
    static class Drawing
    {
        private readonly static Image blinkyImage = Pacman.DesktopUI.Properties.Resources.blinky;
        private readonly static Image pinkyImage = Pacman.DesktopUI.Properties.Resources.pinky;
        private readonly static Image inkyImage = Pacman.DesktopUI.Properties.Resources.inky;
        private readonly static Image clydeImage = Pacman.DesktopUI.Properties.Resources.clyde;

        private readonly static Image frightenedImage = Pacman.DesktopUI.Properties.Resources.frightened;
        private readonly static Image blinkingImage = Pacman.DesktopUI.Properties.Resources.blinking;

        private static Rectangle CharacterDrawingRect(GameCharacter character)
        {
            return new Rectangle((int)((character.GetX() + 1) * character.Size - (character.Size / 2) - character.Size),
                                 (int)((character.GetY() + 1) * character.Size - (character.Size / 2) - character.Size),
                                 (int)(character.Size),
                                 (int)(character.Size));
        }

        private static Rectangle CoinDrawingRect(Cell cell)
        {
            return new Rectangle((int)((cell.GetX() + 1) * cell.Size - (cell.Size / 8) - cell.Size),
                                 (int)((cell.GetY() + 1) * cell.Size - (cell.Size / 8) - cell.Size),
                                 (int)cell.Size / 4,
                                 (int)cell.Size / 4);
        }

        private static Rectangle WallDrawingRect(Cell cell)
        {
            return new Rectangle((int)((cell.GetX() + 1) * cell.Size - (cell.Size / 2) - cell.Size),
                                 (int)((cell.GetY() + 1) * cell.Size - (cell.Size / 2) - cell.Size),
                                 (int)cell.Size,
                                 (int)cell.Size);
        }

        private static Rectangle PowerUpDrawingRect(Cell cell)
        {
            return new Rectangle((int)((cell.GetX() + 1) * cell.Size - (cell.Size / 4) - cell.Size),
                                 (int)((cell.GetY() + 1) * cell.Size - (cell.Size / 4) - cell.Size),
                                 (int)cell.Size / 2,
                                 (int)cell.Size / 2);
        }

        private static void DrawCell(Cell cell, object sender, PaintEventArgs e)
        {
            switch (cell.Content)
            {
                case Content.Wall: DrawWall(cell, sender, e);
                    break;
                case Content.Coin: DrawCoin(cell, sender, e);
                    break;
                case Content.PowerUp: DrawPowerUp(cell, sender, e);
                    break;
                case Content.Empty: break;
                default: break;
            }
        }

        private static void DrawWall(Cell cell, object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Blue, WallDrawingRect(cell));
        }

        private static void DrawCoin(Cell cell, object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.White, CoinDrawingRect(cell));
        }

        private static void DrawPowerUp(Cell cell, object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.LightYellow, PowerUpDrawingRect(cell));
        }

        private static void DrawPacman(Player pacman, object sender, PaintEventArgs e)
        {
            Image image = Pacman.DesktopUI.Properties.Resources.pacman;
            switch (pacman.Direction)
            {
                case Direction.Up: image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Direction.Down: image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.Left: image.RotateFlip(RotateFlipType.Rotate180FlipY);
                    break;
            }
            e.Graphics.DrawImage(image, CharacterDrawingRect(pacman));
        }

        private static void DrawGhost(Ghost ghost, object sender, PaintEventArgs e)
        {
            Image image;

            if (ghost.Behaviour == Behaviour.Frightened)
            {
                if (ghost.IsChanging)
                {
                    image = blinkingImage;
                }
                else
                {
                    image = frightenedImage;
                }
            }
            else
            {
                switch (ghost.Name)
                {
                    case "Blinky": image = blinkyImage;
                        break;
                    case "Pinky": image = pinkyImage;
                        break;
                    case "Inky": image = inkyImage;
                        break;
                    case "Clyde": image = clydeImage;
                        break;
                    default: image = blinkingImage;
                        break;
                }
            }

            e.Graphics.DrawImage(image, CharacterDrawingRect(ghost));
        }

        private static void DrawLevel(Grid grid, object sender, PaintEventArgs e)
        {
            for (int i = 0; i < grid.Width; i++)
            {
                for (int j = 0; j < grid.Height; j++)
                {
                    DrawCell(grid.Map[i, j], sender, e);
                }
            }
        }

        public static void DrawGame(Game game, object sender, PaintEventArgs e)
        {
            DrawLevel(game.Level, sender, e);
            DrawPacman(game.Player, sender, e);
            foreach (Ghost ghost in game.Ghosts)
            {
                DrawGhost(ghost, sender, e);
            }
        }
    }
}
