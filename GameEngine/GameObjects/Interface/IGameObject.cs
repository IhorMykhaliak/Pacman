namespace Pacman.GameEngine
{
    interface IGameObject
    {
        int GetX();
        int GetY();

        BoundingSquare GetBoundingRect();
    }
}
