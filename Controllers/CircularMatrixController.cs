using Microsoft.AspNetCore.Mvc;

namespace dotnet_deployables.Controllers;

[ApiController]
[Route("api/v1/matrix")]
[Produces("application/json")]
public class CircularMatrixController : ControllerBase
{
    private static readonly Dictionary<Direction, MovementPattern> Movements = new Dictionary<Direction, MovementPattern>
    {
        [Direction.Up] = new(0, -1),
        [Direction.Right] = new(1, 0),
        [Direction.Down] = new(0, 1),
        [Direction.Left] = new(-1, 0)
    };
    
    [HttpGet]
    public IActionResult GetCircularMatrix([FromQuery] int rows, [FromQuery] int cols, [FromQuery] bool clockwise, [FromQuery] int startPosition)
    {
        return Ok();
    }

    private (int x, int y) GetStartingPosition(int rows, int cols, StartPosition position)
    {
        return position switch
        {
            StartPosition.TopLeft => (0, 0),
            StartPosition.TopRight => (cols - 1, 0),
            StartPosition.BottomRight => (cols - 1, rows - 1),
            StartPosition.BottomLeft => (0, rows - 1),
            StartPosition.Centre => ((cols - 1) / 2, (rows - 1) / 2),
            _ => throw new ArgumentException("Invalid start position", nameof(position))
        };
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public enum StartPosition
{
    TopLeft = 0,
    TopRight = 1,
    BottomRight = 2,
    BottomLeft = 3,
    Centre = 4
}

public sealed record MovementPattern(int x, int y);