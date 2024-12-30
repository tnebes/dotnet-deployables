namespace dotnet_deployables.Util;

public sealed class MovementController
{
    private readonly MovementPattern[] _movementPattern;
    private int _currentDirectionIndex;
    private (int X, int Y) _currentPosition;

    private MovementController()
    {
    }

    public MovementController(MovementPattern[] movementPattern, int startingIndex, (int X, int Y) startPosition)
    {
        _movementPattern = movementPattern;
        _currentDirectionIndex = startingIndex;
        _currentPosition = startPosition;
    }

    public (int x, int y) GetNextPosition()
    {
        MovementPattern movement = _movementPattern[_currentDirectionIndex];
        _currentPosition = (_currentPosition.X + movement.X, _currentPosition.Y + movement.Y);
        return _currentPosition;
    }

    public (int x, int y) TryNextPosition()
    {
        MovementPattern movement = _movementPattern[_currentDirectionIndex];
        return (_currentPosition.X + movement.X, _currentPosition.Y + movement.Y);
    }

    public void NextDirection()
    {
        _currentDirectionIndex = (_currentDirectionIndex + 1) % _movementPattern.Length;
    }

    public void SetPosition((int X, int Y) newPosition)
    {
        _currentPosition = newPosition;
    }
}