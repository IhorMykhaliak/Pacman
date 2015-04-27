using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    public interface IPathfindingAlgorithm
    {
        List<Cell> CalculatePath(Cell startCell, Cell endCell, Cell[,] levelGrid);
    }
}
