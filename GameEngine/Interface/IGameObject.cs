using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    interface IGameObject
    {
        int GetX();
        int GetY();

        BoundingSquare GetBoundingRect();
    }
}
