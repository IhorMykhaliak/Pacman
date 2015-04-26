using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman.GameEngine
{
    public class Clyde : Ghost
    {
        #region Initialization

        public Clyde(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            _name = "Clyde";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = AStarAlgorithm.CalculatePath(StartCell, _level.Map[8, 27], _level.Map);
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[8, 27], _level.Map[12, 30], _level.Map));
            PatrolPath.AddRange(AStarAlgorithm.CalculatePath(_level.Map[12, 30], StartCell, _level.Map));
        }

        #endregion

        #region Behaviour

        public override void UpdateChasePath()
        {
            int xDistance, yDistance, distance;

            xDistance = Math.Abs(GetX() - _pacman.GetX());
            yDistance = Math.Abs(GetY() - _pacman.GetY());
            distance = xDistance + yDistance;

            if (distance > 8)
            {
                base.UpdateChasePath();
            }
            else
            {
                UseStupidPath();
            }
        }

        public void UseStupidPath()
        {
            List<Cell> bestPath = AStarAlgorithm.CalculatePath(CurrentCell(), _level.GetRandomFreeCell(), _level.Map);
            _pathIterator = 0;

            SelectChasePath(bestPath);

            _targetCell = _chasePath.Last();
        }

        #endregion
    }
}
