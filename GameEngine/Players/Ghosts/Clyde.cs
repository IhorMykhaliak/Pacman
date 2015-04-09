using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    class Clyde : Ghost
    {
        public Clyde(Grid grid, int x, int y, float size)
            : base(grid, x, y, size)
        {
            _name = "Clyde";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[8, 27], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[8, 27], _level.Map[12, 30], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[12, 30], StartCell, _level.Map));
        }

        public override void UpdateChasePath(Player pacman)
        {
            int xDistance, yDistance, distance;

            xDistance = Math.Abs(GetX() - pacman.GetX());
            yDistance = Math.Abs(GetY() - pacman.GetY());
            distance = xDistance + yDistance;

            if (distance > 8)
            {
                UseNormalPath(pacman);
            }
            else
            {
                UseStupidPath(pacman);
            }
        }

        private void UseNormalPath(Player pacman)
        {
            List<Cell> bestPath = AStarAlgorithm.CalculatePath(CurrentCell(), pacman.CurrentCell(), _level.Map);
            _pathIterator = 0;

            if (bestPath.Count >= chasePathLength)
            {
                _chasePath = bestPath.GetRange(0, chasePathLength);
            }
            else
            {
                _chasePath = bestPath;
            }

            _targetCell = _chasePath.Last();
        }

        private void UseStupidPath(Player pacman)
        {
            List<Cell> bestPath = AStarAlgorithm.CalculatePath(CurrentCell(), _level.GetRandomEmptyCell(), _level.Map);
            _pathIterator = 0;

            if (bestPath.Count >= chasePathLength)
            {
                _chasePath = bestPath.GetRange(0, chasePathLength);
            }
            else
            {
                _chasePath = bestPath;
            }

            _targetCell = _chasePath.Last();
        }
    }
}
