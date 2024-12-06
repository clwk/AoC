namespace Aoc2024.Days;

using Microsoft.Extensions.Logging;

public class Day06
{
    private readonly ILogger _logger;

    private readonly Dictionary<(int row, int col), char> _board;
    private readonly HashSet<(int row, int col)> _visitedPositions = [];

    private Guard _guard;

    // Part 2
    private readonly HashSet<(int row, int col)> _loopPositions = [];
    private readonly IEnumerator<KeyValuePair<(int row, int col), char>> _boardEnumerator;
    private readonly (int row, int col) _startPos;
    private (int row, int col)? _obstaclePosition;

    public Day06(string[] input, ILogger logger)
    {
        _logger = logger;
        logger.LogInformation("Starting parsing");
        _board = ParseToDictionary(input);
        _boardEnumerator = _board.GetEnumerator();

        _startPos = _board.Single(b => b.Value == '^').Key;
        _guard = new Guard(_startPos);

        _obstaclePosition = GetNextObstaclePosition();
    }

    private static Dictionary<(int row, int col), char> ParseToDictionary(string[] input)
    {
        var dictionary = new Dictionary<(int row, int col), char>();

        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                dictionary[(row, col)] = input[row][col];
            }
        }

        return dictionary;
    }

    public int RunPart1()
    {
        RunGame();
        return _visitedPositions.Count;
    }

    private void RunGame()
    {
        _visitedPositions.Clear();
        _visitedPositions.Add(_startPos);

        var count = 0;
        var oldVisitedCount = 0;

        var isLoop = false;
        while (
            _board.TryGetValue(
                _guard.GetNextPositionInDirection(_guard.Direction4Way),
                out var boardValue
            ) && !isLoop
        )
        {
            count++;

            if (boardValue is '.' or '^')
            {
                _guard.Move();
                _visitedPositions.Add(_guard.Position);
            }
            else
            {
                _guard.Rotate();
            }

            if (count % 100 == 0)
            {
                var newVisitedCount = _visitedPositions.Count;
                if (newVisitedCount == oldVisitedCount)
                    isLoop = true;

                oldVisitedCount = newVisitedCount;
            }
        }

        if (isLoop)
        {
            _logger.LogInformation("Loop for position {@pos}", _obstaclePosition);
            _loopPositions.Add(_obstaclePosition!.Value);
        }
    }

    public int RunPart2()
    {
        _logger.LogInformation("Obstacle position {@position}", _obstaclePosition);

        while (_obstaclePosition != null)
        {
            _guard = new Guard(_startPos);
            RunGame();

            // Remove obstacle from board
            _board[_obstaclePosition.Value] = '.';
            _obstaclePosition = GetNextObstaclePosition();
            _logger.LogInformation("Obstacle position {@position}", _obstaclePosition);
            if (_obstaclePosition is not null)
                // add obstacle to new position on board
                _board[_obstaclePosition.Value] = '#';
        }

        return _loopPositions.Count;
    }

    private (int row, int col)? GetNextObstaclePosition()
    {
        var nextPos = GetNextPossibleObstaclePosition();

        while (nextPos is not null && nextPos.Value.Value is not '.')
        {
            nextPos = GetNextPossibleObstaclePosition();
        }

        return nextPos?.Key ?? null;
    }

    private KeyValuePair<(int, int), char>? GetNextPossibleObstaclePosition()
    {
        if (_boardEnumerator.MoveNext())
        {
            return _boardEnumerator.Current;
        }

        return null; // No more elements
    }
}

internal class Guard
{
    public Guard((int row, int col) startPosition)
    {
        Position = startPosition;
        Direction4Way = Direction4Way.Up;
    }

    public (int row, int col) Position { get; private set; }
    public Direction4Way Direction4Way { get; private set; }

    public void Move()
    {
        Position = GetNextPositionInDirection(Direction4Way);
    }

    public void Rotate()
    {
        Direction4Way = GetNextDirection(Direction4Way);
    }

    public (int row, int col) GetNextPositionInDirection(Direction4Way direction4Way)
    {
        var nextRelative = GetNextRelativePosition(direction4Way);
        return (Position.row + nextRelative.row, Position.col + nextRelative.col);
    }

    private static (int row, int col) GetNextRelativePosition(Direction4Way direction4Way) =>
        direction4Way switch
        {
            Direction4Way.Up => (-1, 0),
            Direction4Way.Right => (0, 1),
            Direction4Way.Down => (1, 0),
            Direction4Way.Left => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction4Way), direction4Way, null),
        };

    private static Direction4Way GetNextDirection(Direction4Way direction4Way) =>
        direction4Way switch
        {
            Direction4Way.Up => Direction4Way.Right,
            Direction4Way.Right => Direction4Way.Down,
            Direction4Way.Down => Direction4Way.Left,
            Direction4Way.Left => Direction4Way.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction4Way), direction4Way, null),
        };
}

internal enum Direction4Way
{
    Up,
    Right,
    Down,
    Left,
}
