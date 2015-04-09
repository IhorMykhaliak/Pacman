using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public class Grid
    {
        #region Private fields

        private Random random = new Random();

        private readonly float _cellSize;
        private int _width;
        private int _height;
        private int _coins;
        private Cell[,] _map;

        #endregion

        #region Properties

        public int Coins
        {
            get
            {
                return _coins;
            }
        }

        public Cell[,] Map
        {
            get
            {
                return _map;
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        #endregion

        #region Initialization

        public Grid()
        {
            _map = new Cell[_width, _height];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _map[i, j] = new Cell((i + 1) * _cellSize, (j + 1) * _cellSize);
                }
            }

            _coins = 0;
        }

        public Grid(char[,] mapStructure, float size)
        {
            _cellSize = size;
            _width = mapStructure.GetLength(0);
            _height = mapStructure.GetLength(1);
            _map = new Cell[_width, _height];

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    switch (mapStructure[j, i])
                    {
                        case 'w': _map[i, j] = new Cell((i + 1) * _cellSize, (j + 1) * _cellSize, Content.Wall, _cellSize);
                            break;
                        case 'c': _map[i, j] = new Cell((i + 1) * _cellSize, (j + 1) * _cellSize, Content.Coin, _cellSize);
                            _coins++;
                            break;
                        case 'p': _map[i, j] = new Cell((i + 1) * _cellSize, (j + 1) * _cellSize, Content.PowerUp, _cellSize);
                            _coins++;
                            break;
                        case 'e': _map[i, j] = new Cell((i + 1) * _cellSize, (j + 1) * _cellSize, Content.Empty, _cellSize);
                            break;
                    }
                }
            }
        }

        #endregion

        public Cell GetRandomEmptyCell()
        {
            Cell randomCell = new Cell();
            randomCell.Content = Content.Wall;

            while (randomCell.IsWall())
            {
                int x = random.Next(0, _width);
                int y = random.Next(0, _height);
                randomCell = _map[x, y];
            }

            return randomCell;
        }
    }
}
