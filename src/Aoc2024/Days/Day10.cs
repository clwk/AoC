using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day10
{
    private readonly ILogger _logger;

    private readonly Dictionary<(int row, int col), int> _grid;
    private Dictionary<(int row, int col), int>.Enumerator _gridEnumerator;

    private readonly Dictionary<(int row, int col), int> _trailHeadScore = [];

    public Day10(string[] input, ILogger logger)
    {
        _logger = logger;

        _grid = ParseInput(input);
        _gridEnumerator = _grid.GetEnumerator();
    }

    private Dictionary<(int row, int col), int> ParseInput(string[] input)
    {
        var grid = new Dictionary<(int row, int col), int>();

        for (var row = 0; row < input.Length; row++)
        {
            var line = input[row];

            for (var col = 0; col < line.Length; col++)
            {
                if (int.TryParse(line[col].ToString(), out int value))
                {
                    grid[(row, col)] = value;
                }
                else
                {
                    _logger.LogWarning(
                        "Invalid integer value at row {row}, col {col}: {value}",
                        row,
                        col,
                        line[col]
                    );
                }
            }
        }

        return grid;
    }

    public int RunPart1()
    {
        var visited = new Dictionary<(int, int), HashSet<(int, int)>>();

        while (_gridEnumerator.MoveNext())
        {
            var currentPosVal = _gridEnumerator.Current;
            if (currentPosVal.Value == 9)
            {
                if (!visited.TryGetValue(currentPosVal.Key, out HashSet<(int, int)>? value))
                {
                    value = new HashSet<(int, int)>();
                    visited[currentPosVal.Key] = value;
                }

                var stack = new Stack<KeyValuePair<(int, int), int>>();
                stack.Push(currentPosVal);

                while (stack.Count > 0)
                {
                    var posVal = stack.Pop();
                    value.Add(posVal.Key);

                    var nextPositions = GetNextValidPositions(posVal, value);
                    foreach (var nextPos in nextPositions)
                    {
                        if (!visited[currentPosVal.Key].Contains(nextPos.Key))
                        {
                            stack.Push(nextPos);
                        }
                    }

                    if (nextPositions.Count == 0 && posVal.Value == 0)
                    {
                        AddScore(posVal.Key);
                        _logger.LogInformation("found at {@pos}", posVal.Key);
                    }
                }
            }
            else
            {
                _logger.LogDebug(
                    "{position} is not the top of a trail.",
                    _gridEnumerator.Current.Key
                );
            }
        }

        return _trailHeadScore.Sum(kvp => kvp.Value);
    }

    public int RunPart2()
    {
        _gridEnumerator = _grid.GetEnumerator();
        var distinctPaths = new List<List<(int row, int col)>>();

        while (_gridEnumerator.MoveNext())
        {
            var currentPosVal = _gridEnumerator.Current;
            if (currentPosVal.Value == 9)
            {
                var stack =
                    new Stack<(KeyValuePair<(int, int), int> posVal, List<(int, int)> path)>();
                stack.Push((currentPosVal, [currentPosVal.Key]));

                while (stack.Count > 0)
                {
                    var (posVal, path) = stack.Pop();

                    var nextPositions = GetNextValidPositions(posVal, [.. path]);
                    foreach (var nextPos in nextPositions)
                    {
                        var newPath = new List<(int, int)>(path) { nextPos.Key };
                        if (nextPos.Value == 0)
                        {
                            distinctPaths.Add(newPath);
                            _logger.LogInformation(
                                "Path found: {path}",
                                string.Join("->", newPath)
                            );
                        }
                        else
                        {
                            stack.Push((nextPos, newPath));
                        }
                    }
                }
            }
            else
            {
                _logger.LogDebug(
                    "{position} is not the top of a trail.",
                    _gridEnumerator.Current.Key
                );
            }
        }

        return distinctPaths.Count;
    }

    private void AddScore((int, int) position)
    {
        if (_trailHeadScore.TryGetValue(position, out var value))
            _trailHeadScore[position] = value + 1;
        else
        {
            _trailHeadScore[position] = 1;
        }
    }

    private List<KeyValuePair<(int row, int col), int>> GetNextValidPositions(
        KeyValuePair<(int row, int col), int> currentPosVal,
        HashSet<(int, int)> visited
    )
    {
        var validPositions = new List<KeyValuePair<(int row, int col), int>>();

        foreach (var direction in Enum.GetValues<Direction>())
        {
            var nextPos = GetNextPos(currentPosVal.Key, direction);

            if (_grid.TryGetValue(nextPos, out var newValue) && !visited.Contains(nextPos))
            {
                if (currentPosVal.Value - newValue == 1)
                {
                    validPositions.Add(
                        new KeyValuePair<(int row, int col), int>(nextPos, newValue)
                    );
                }
            }
        }

        return validPositions;
    }

    private (int, int) GetNextPos((int, int) currentPos, Direction direction) =>
        currentPos.Add(GetNextRelativePosition(direction));

    private (int row, int col) GetNextRelativePosition(Direction direction) =>
        direction switch
        {
            Direction.Up => (-1, 0),
            Direction.Right => (0, 1),
            Direction.Down => (1, 0),
            Direction.Left => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(Direction), direction, null),
        };
}

enum Direction
{
    Right,
    Down,
    Left,
    Up,
};
