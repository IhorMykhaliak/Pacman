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

        // 1 problem with threads
        // 2 sounds in tests 
        public GameForm()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        #region Event handlers

        private void GameLoad(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            _game = new Game();
            GameSubscribe();

            Paint += Draw;
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
                case Keys.Space: _game.PauseGame();
                    break;
                case Keys.R: Restart();
                    break;
            }
        }

        private void Restart()
        {
            Paint -= Draw;
            GameUnsubscribe();

            _game.Dispose();
            _game = new Game();
            
            _game.IsPaused = false;
            menu.Visible = false;
            _game.MainTimer.Enabled = true;
            
            Paint += Draw;
            GameSubscribe();
        }

        private void Pause()
         {
            menu.Visible = _game.IsPaused;
         }

        private void UpdateGame()
        {
            countLabel.Text = _game.Score.ToString();
            Refresh();
        }

        private void PlayerWin()
        {
            Restart();
            _game.PauseGame();
            MessageBox.Show("You won !");
        }

        private void PlayerDie()
        {
            _game.PauseGame();
            Restart();
            _game.PauseGame();
            MessageBox.Show("You died !");
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            Drawing.DrawGame(_game, sender, e);
        }

        #endregion

        private void GameSubscribe()
        {
            _game.Update += UpdateGame;
            _game.Pause += Pause;
            _game.Win += PlayerWin;
            _game.Die += PlayerDie;
        }

        private void GameUnsubscribe()
        {
            _game.Update -= UpdateGame;
            _game.Pause -= Pause;
            _game.Win -= PlayerWin;
            _game.Die -= PlayerDie;
        }
    }
}
