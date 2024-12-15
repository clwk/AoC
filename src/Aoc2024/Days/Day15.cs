using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;

namespace Aoc2024.Days;

public class Day15
{
    private readonly ILogger _logger;
    private readonly string[] _input;

    private Dictionary<(int row, int col), char> _grid = [];
    private (int row, int col) _dimensions;

    private List<char> _moves = [];
    private List<Direction4Way> _movesDirections = [];

    private (int row, int col) _robotPos;
    private List<Direction4Way>.Enumerator _movesEnumerator;

    public Day15(string[] input, ILogger logger)
    {
        _input = input;
        _logger = logger;
        ParseInput();

        ParseMovesToDirections();

        _robotPos = _grid.First(g => g.Value == '@').Key;
        _grid[_robotPos] = '.';
        _logger.LogDebug("Robot start position: {@pos}", _robotPos);

        _movesEnumerator = _movesDirections.GetEnumerator();
    }

    public int RunPart1()
    {
        TryMove();

        return CalculatePoints();
    }

    private int CalculatePoints()
    {
        return _grid
            .Where(cell => cell.Value == 'O')
            .Sum(cell => cell.Key.row * 100 + cell.Key.col);
    }

    private void TryMove()
    {
        _logger.LogDebug("Enumerator position: {@pos}", _movesEnumerator.Current);

        while (_movesEnumerator.MoveNext())
        {
            var nextPos = GetNextCandidatePos(_robotPos, _movesEnumerator.Current);

            if (nextPos.Value == '.')
                _robotPos = nextPos.Key;
            else if (nextPos.Value == 'O')
            {
                var spaces = FindPositionsUntilNextSpace(_robotPos, _movesEnumerator.Current);
                if (spaces is { Count: > 0 })
                {
                    MoveToSpace(spaces);
                }
            }

            _logger.LogDebug("Robot position: {@pos}", _robotPos);

            PrintGridWithRobot();
        }
    }

    private void MoveToSpace(List<KeyValuePair<(int row, int col), char>> spaces)
    {
        _robotPos = spaces.First().Key;
        _grid[spaces.Last().Key] = 'O';
        _grid[_robotPos] = '.';
    }

    private KeyValuePair<(int row, int col), char> GetNextCandidatePos(
        (int row, int col) position,
        Direction4Way direction
    )
    {
        var nextPos = direction switch
        {
            Direction4Way.Up => position.Add((-1, 0)),
            Direction4Way.Right => position.Add((0, 1)),
            Direction4Way.Down => position.Add((1, 0)),
            Direction4Way.Left => position.Add((0, -1)),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };

        return _grid.First(kvp => kvp.Key == nextPos);
    }

    private List<KeyValuePair<(int row, int col), char>>? FindPositionsUntilNextSpace(
        (int row, int col) position,
        Direction4Way direction
    )
    {
        var searchSpace = direction switch
        {
            Direction4Way.Right => _grid
                .Where(g => g.Key.row == position.row && g.Key.col > position.col)
                .OrderBy(x => x.Key.col)
                .TakeUntil(s => s.Value == '.'),
            Direction4Way.Left => _grid
                .Where(g => g.Key.row == position.row && g.Key.col < position.col)
                .OrderByDescending(x => x.Key.col)
                .TakeUntil(s => s.Value == '.'),
            Direction4Way.Up => _grid
                .Where(g => g.Key.col == position.col && g.Key.row < position.row)
                .OrderByDescending(x => x.Key.row)
                .TakeUntil(s => s.Value == '.'),
            Direction4Way.Down => _grid
                .Where(g => g.Key.col == position.col && g.Key.row > position.row)
                .OrderBy(x => x.Key.row)
                .TakeUntil(s => s.Value == '.'),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };

        // if contains # - no . was found
        var positionsUntilNextSpace = searchSpace.ToList();
        var searchHitWall = positionsUntilNextSpace.Any(s => s.Value == '#');

        return searchHitWall ? null : positionsUntilNextSpace;
    }

    private void ParseInput()
    {
        _grid = new Dictionary<(int row, int col), char>();
        _moves = [];

        var isGrid = true;
        var row = 0;

        foreach (var line in _input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                isGrid = false;
                continue;
            }

            if (isGrid)
            {
                for (int col = 0; col < line.Length; col++)
                {
                    _grid[(row, col)] = line[col];
                }
                row++;
            }
            else
            {
                _moves.AddRange(line);
            }
        }
        _dimensions = (row - 1, _grid.Keys.Max(k => k.col));
    }

    private void ParseMovesToDirections()
    {
        _movesDirections = _moves.Select(ConvertToDirection).ToList();
    }

    private static Direction4Way ConvertToDirection(char move) =>
        move switch
        {
            '>' => Direction4Way.Right,
            '<' => Direction4Way.Left,
            '^' => Direction4Way.Up,
            'v' => Direction4Way.Down,
            _ => throw new ArgumentException($"Invalid move character: {move}"),
        };

    private void PrintGridWithRobot()
    {
        var grid = new char[_dimensions.row + 1, _dimensions.col + 1];

        // Initialize the grid with the current state
        foreach (var cell in _grid)
        {
            grid[cell.Key.row, cell.Key.col] = cell.Value;
        }

        // Overwrite the robot position with '@'
        grid[_robotPos.row, _robotPos.col] = '@';

        // Print the grid using _logger
        for (var row = 0; row <= _dimensions.row; row++)
        {
            var line = new char[_dimensions.col + 1];
            for (var col = 0; col <= _dimensions.col; col++)
            {
                line[col] = grid[row, col];
            }
            _logger.LogDebug("{line}", new string(line));
        }
        _logger.LogDebug("END------------------------");
    }

    public int RunPart2()
    {
        return 0;
    }
}
