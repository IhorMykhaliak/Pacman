using System.Linq;

namespace Pacman.GameEngine
{
    public class Blinky : Ghost
    {
        #region Initialization

        public Blinky(Player pacman, Grid grid, int x, int y, float size)
            : base(pacman, grid, x, y, size)
        {
            _name = "Blinky";
        }

        public override void InitializePatrolPath()
        {
            PatrolPath = _algorithm.CalculatePath(StartCell, _level.Map[24, 6], _level.Map);
            PatrolPath.AddRange(_algorithm.CalculatePath(_level.Map[24, 6], _level.Map[28, 6], _level.Map));
            PatrolPath.AddRange(_algorithm.CalculatePath(_level.Map[28, 6], StartCell, _level.Map));
        }

        #endregion

        #region Behaviour

        public override void UpdateChasePath()
        {
            base.UpdateChasePath();

            CheckTunnel();
        }

        private void CheckTunnel()
        {
            if (_pacman.IsPassedRightTunnel)
            {
                ChaseRightTunnel();
            }
            else
                if (_pacman.IsPassedLeftTunnel)
                {
                    ChaseLeftTunnel();
                }
                else
                {
                    _targetCell = _chasePath.Last();
                }
        }

        private void ChaseLeftTunnel()
        {
            _chasePath = _algorithm.CalculatePath(CurrentCell(), _level.Map[0, 15], _level.Map);
            _targetCell = _level.Map[32, 15];
            _pacman.IsPassedLeftTunnel = false;
        }

        private void ChaseRightTunnel()
        {
            _chasePath = _algorithm.CalculatePath(CurrentCell(), _level.Map[33, 15], _level.Map);
            _targetCell = _level.Map[0, 15];
            _pacman.IsPassedRightTunnel = false;
        }

        #endregion
    }
}
