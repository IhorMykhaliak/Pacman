using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;

namespace Pacman.GameEngine
{
    public class Player : GameCharacter
    {
        #region Private fields

        private const float powerUpTime = 5.0f;

        private int _coins;
        private bool _isPoweredUp;
        private float _powerUpTime;

        #endregion

        #region Properties

        public float PowerUpTime
        {
            get
            {
                return _powerUpTime;
            }
            set
            {
                _powerUpTime = value;
            }
        }

        public bool IsPoweredUp
        {
            get
            {
                return _isPoweredUp;
            }
            set
            {
                _isPoweredUp = value;
            }
        }

        public int Coins
        {
            get
            {
                return _coins;
            }
        }

        #endregion

        #region Initialization

        public Player()
            : base()
        {
            _coins = 0;
            _isPoweredUp = false;
            PowerUpTime = 5.0f;
        }

        public Player(Grid grid)
            : this()
        {
            _level = grid;
        }

        public Player(Grid grid, int x, int y, float size)
            : this(grid)
        {
            _size = size;
            SetX(x);
            SetY(y);
        }

        #endregion

        #region Pick item

        public void PickItem(List<Ghost> ghosts)
        {
            TryPickItem(_level.Map[GetX(), GetY()], ghosts);
        }

        private void TryPickItem(Cell cell, List<Ghost> ghosts)
        {
            if (IsInCell(cell))
            {
                TryPickCoin(cell);
                TryPickPowerUp(cell, ghosts);
            }
        }

        private bool IsInCell(Cell cell)
        {
            return GetBoundingRect() == cell.GetBoundingRect();
        }

        private void TryPickCoin(Cell cell)
        {
            if (cell.IsCoin())
            {
                PlaySoundEatItem();
                UpdateAfterPickUp(cell);
            }
        }

        private void TryPickPowerUp(Cell cell, List<Ghost> ghosts)
        {
            if (cell.IsPowerUp())
            {
                PlaySoundEatItem();

                UpdateAfterPickUp(cell);

                foreach (var ghost in ghosts)
                {
                    ghost.TargetCell = ghost.CurrentCell();
                }

                _isPoweredUp = true;
                PowerUpTime = powerUpTime;
            }
        }

        private void UpdateAfterPickUp(Cell cell)
        {
            cell.Content = Content.Empty;
            _coins++;
        }

        #endregion

        private void PlaySoundEatItem()
        {
            SoundPlayer player = new SoundPlayer(Pacman.GameEngine.Properties.Resources.pacman_chomp);
            player.Play();
        }
    }
}
