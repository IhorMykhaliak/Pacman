using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pacman.GameEngine;

namespace Pacman.DesktopUI
{
    public partial class GameForm : Form
    {
        private Game _game;

        public GameForm()
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void GameLoad(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            _game = new Game();

            Paint += Draw;
            _game.Update += Refresh;
            menu.Visible = true;
        }

        private void GameFormKeyDown(object sender, KeyEventArgs e)
        {
            _game.Player.PreviousDirection = _game.Player.Direction;
            _game.Player.PendingDirection = Direction.None;

            switch (e.KeyData)
            {
                case Keys.Up: _game.Player.Direction = Direction.Up;
                    break;
                case Keys.Down: _game.Player.Direction = Direction.Down;
                    break;
                case Keys.Left: _game.Player.Direction = Direction.Left;
                    break;
                case Keys.Right: _game.Player.Direction = Direction.Right;
                    break;
                case Keys.Space: Pause();
                    break;
                case Keys.R: Restart();
                    break;
            }
        }

        private void Restart()
        {
            Paint -= Draw;
            _game.Update -= Refresh;
            _game.Dispose();
            _game = new Game();
            _game.IsPaused = false;
            menu.Visible = false;
            _game.MainTimer.Enabled = true;
            Paint += Draw;
            _game.Update += Refresh;
        }

        private void Pause()
         {
            _game.MainTimer.Enabled = _game.IsPaused;
            _game.IsPaused = !_game.IsPaused;
            menu.Visible = _game.IsPaused;
         }

        private void Draw(object sender, PaintEventArgs e)
        {
            Drawing.DrawLevel(_game.Level, sender, e);
            Drawing.DrawPacman(_game.Player, sender, e);
            foreach (Ghost ghost in _game.Ghosts)
            {
                Drawing.DrawGhost(ghost, sender, e);
            }
        }
    }
}
