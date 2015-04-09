using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameEngine
{
    class Blinky : Ghost
    {
        public Blinky(Grid grid, int x, int y, float size)
            : base(grid, x, y, size)
        {
            _name = "Blinky";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[24, 6], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[24, 6], _level.Map[28, 6], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[28, 6], StartCell, _level.Map));
        }

        public override void UpdateChasePath(Player pacman)
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

            CheckTunnel(pacman);
        }

        private void CheckTunnel(Player pacman)
        {
            if (pacman.IsPassedRightTunnel)
            {
                _chasePath = AStarAlgorithm.CalculatePath(CurrentCell(), _level.Map[33, 15], _level.Map);
                _targetCell = _level.Map[0, 15];
                pacman.IsPassedRightTunnel = false;
            }
            else
                if (pacman.IsPassedLeftTunnel)
                {
                    _chasePath = AStarAlgorithm.CalculatePath(CurrentCell(), _level.Map[0, 15], _level.Map);
                    _targetCell = _level.Map[32, 15];
                    pacman.IsPassedLeftTunnel = false;
                }
                else
                {
                    _targetCell = _chasePath.Last();
                }
        }
    }
}
