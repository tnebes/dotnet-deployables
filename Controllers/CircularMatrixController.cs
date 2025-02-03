#region

using dotnet_deployables.Util;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace dotnet_deployables.Controllers;

[ApiController]
[Route("api/v1/matrix")]
[Produces("application/json")]
public sealed class CircularMatrixController : ControllerBase
{
    private static readonly Dictionary<Direction, MovementPattern> Movements = new()
    {
        [Direction.Up] = new MovementPattern(0, -1),
        [Direction.Right] = new MovementPattern(1, 0),
        [Direction.Down] = new MovementPattern(0, 1),
        [Direction.Left] = new MovementPattern(-1, 0)
    };

    private static readonly MovementPattern[] MovementPattern =
    [
        Movements[Direction.Up],
        Movements[Direction.Right],
        Movements[Direction.Down],
        Movements[Direction.Left]
    ];

    [HttpGet]
    public IActionResult GetCircularMatrix([FromQuery] int rows, [FromQuery] int cols, [FromQuery] int startPosition,
        [FromQuery] bool clockwise, [FromQuery] bool startFromOne)
    {
        try
        {
            if (rows <= 0 || cols <= 0) return BadRequest("Rows and columns must be greater than 0");
            if (startPosition is < 0 or > 4) return BadRequest("Invalid start position");

            int[,] matrix = new int[rows, cols];
            (int x, int y) = GetStartingPosition(rows, cols, (StartPosition)startPosition);

            if (startPosition == (int)StartPosition.Centre)
                FillCenterSpiral(matrix, x, y, clockwise, startFromOne);
            else
                FillRegularSpiral(matrix, x, y, startPosition, clockwise, startFromOne);

            var result = new
            {
                rows,
                cols,
                matrix = ConvertToJaggedArray(matrix)
            };

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    private void FillCenterSpiral(int[,] matrix, int startX, int startY, bool clockwise, bool startFromOne)
    {
        MovementController movementController = new(MovementPattern, clockwise ? 1 : 3, (startX, startY));
        ValueController valueController = new(matrix.GetLength(0) * matrix.GetLength(1), startFromOne);

        int currentX = startX;
        int currentY = startY;
        int minX = startX - 1;
        int minY = startY - 1;
        int maxX = startX + 1;
        int maxY = startY + 1;

        matrix[currentY, currentX] = valueController.GetValue();

        int retries = 0;

        while (!valueController.IsDone())
        {
            (int nextX, int nextY) = movementController.TryNextPosition();

            if (nextX > maxX || nextX < minX || nextY > maxY || nextY < minY || matrix[nextY, nextX] != 0)
            {
                if (retries > 4)
                    throw new MatrixGenerationException("The circular matrix of size " + matrix.GetLength(0) + "x" +
                                                        matrix.GetLength(1) + " is not possible.");

                movementController.NextDirection();
                retries++;
                if (nextX == maxX && nextX < matrix.GetLength(1) - 1) maxX++;
                else if (nextX == minX && minX > 0) minX--;
                else if (nextY == maxY && nextY < matrix.GetLength(0) - 1) maxY++;
                else if (nextY == minY && nextY > 0) minY--;
            }
            else
            {
                (currentX, currentY) = movementController.GetNextPosition();
                matrix[currentY, currentX] = valueController.GetValue();
                retries = 0;
            }
        }
    }

    private void FillRegularSpiral(int[,] matrix, int x, int y, int startPosition, bool clockwise, bool startFromOne)
    {
        int startingMovementIndex = clockwise ? (startPosition + 1) % 4 : (startPosition + 2) % 4;
        MovementController movementController = new(MovementPattern, startingMovementIndex, (x, y));
        ValueController valueController = new(matrix.GetLength(0) * matrix.GetLength(1), startFromOne);

        while (!valueController.IsDone())
        {
            int value = valueController.GetValue();
            matrix[y, x] = value;

            if (valueController.IsDone()) break;

            (int nextX, int nextY) = movementController.TryNextPosition();
            while (nextX < 0 || nextX >= matrix.GetLength(1) ||
                   nextY < 0 || nextY >= matrix.GetLength(0) ||
                   matrix[nextY, nextX] != 0)
            {
                movementController.NextDirection();
                (nextX, nextY) = movementController.TryNextPosition();
            }

            (x, y) = movementController.GetNextPosition();
        }
    }

    private static int[][] ConvertToJaggedArray(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        int[][] jaggedArray = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            jaggedArray[i] = new int[cols];
            for (int j = 0; j < cols; j++) jaggedArray[i][j] = matrix[i, j];
        }

        return jaggedArray;
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