using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day14
{
    private readonly ILogger _logger;
    private readonly string[] _input;
    List<Robot> _robots = [];
    private (int x, int y) _dimensions;

    public Day14(string[] input, ILogger logger)
    {
        _input = input;
        _logger = logger;
    }

    private List<Robot> ParseInput(string[] input)
    {
        var robots = new List<Robot>();

        foreach (var line in input)
        {
            var parts = line.Split(' ');
            var positionParts = parts[0].Substring(2).Split(',');
            var velocityParts = parts[1].Substring(2).Split(',');

            var position = (x: int.Parse(positionParts[0]), y: int.Parse(positionParts[1]));
            var velocity = (x: int.Parse(velocityParts[0]), y: int.Parse(velocityParts[1]));

            robots.Add(new Robot(position, velocity, _dimensions));
        }

        return robots;
    }

    public int RunPart1((int x, int y) dimensions)
    {
        _dimensions = dimensions;
        _robots = ParseInput(_input);
        PrintRobotPositions(0);

        RunGame(100, int.MaxValue, int.MaxValue);

        PrintRobotPositions(100);
        return CalculateScore();
    }

    public int RunPart2((int x, int y) dimensions)
    {
        _dimensions = dimensions;
        _robots = ParseInput(_input);

        return RunGame(10000, 20, 20);
    }

    private int RunGame(int maxIter, int xLimit, int yLimit)
    {
        var iterationCount = 0;
        for (var i = 1; i <= maxIter; i++)
        {
            foreach (var robot in _robots)
            {
                robot.Move();
                if (robot.Velocity == (2, -3))
                    _logger.LogDebug(
                        "{sec} s: Robot {velocity} now at {pos}",
                        i,
                        robot.Velocity,
                        robot.Position
                    );
            }

            if (
                AreMoreThanXRobotsInSameYPosition(yLimit)
                && AreMoreThanXRobotsInSameXPosition(xLimit)
            )
            {
                iterationCount = i;
                PrintRobotPositions(i);
                break;
            }
        }

        return iterationCount;
    }

    private bool AreMoreThanXRobotsInSameXPosition(int x)
    {
        var robotsInSameXPosition = _robots.GroupBy(r => r.Position.x).Any(g => g.Count() > x);

        return robotsInSameXPosition;
    }

    private bool AreMoreThanXRobotsInSameYPosition(int y)
    {
        var robotsInSameXPosition = _robots.GroupBy(r => r.Position.y).Any(g => g.Count() > y);

        return robotsInSameXPosition;
    }

    private int CalculateScore()
    {
        var min = (x: 0, y: 0);
        var max = (_dimensions.x, _dimensions.y);
        var score = 1;

        // Iterate over each quadrant, excluding the middle row and column
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var quadrantMin = (x: min.x + i * (max.x + 1) / 2, y: min.y + j * (max.y + 1) / 2);
                var quadrantMax = (
                    x: min.x + (i + 1) * (max.x + 1) / 2 - 2,
                    y: min.y + (j + 1) * (max.y + 1) / 2 - 2
                );

                // Perform calculations for each quadrant
                score *= CalculateQuadrantScore(quadrantMin, quadrantMax);
            }
        }

        return score;
    }

    private int CalculateQuadrantScore((int x, int y) min, (int x, int y) max)
    {
        var quadrantScore = _robots.Count(r =>
            r.Position.x >= min.x
            && r.Position.y >= min.y
            && r.Position.x <= max.x
            && r.Position.y <= max.y
        );

        return quadrantScore;
    }

    private void PrintRobotPositions(int second)
    {
        var grid = new int[_dimensions.x + 1, _dimensions.y + 1];

        // Initialize the grid with robot counts
        foreach (var robot in _robots)
        {
            grid[robot.Position.x, robot.Position.y]++;
        }

        _logger.LogInformation("Second: {i}", second);
        // Print the grid
        for (var y = 0; y <= _dimensions.y; y++)
        {
            Console.WriteLine(BuildLine(grid, y));
            _logger.LogInformation(BuildLine(grid, y));
        }
        _logger.LogInformation("END");
    }

    private string BuildLine(int[,] grid, int y)
    {
        var line = new char[_dimensions.x + 1];
        for (var x = 0; x <= _dimensions.x; x++)
        {
            line[x] = grid[x, y] > 0 ? (char)('0' + grid[x, y]) : '.';
        }
        return new string(line);
    }
}

internal class Robot
{
    private readonly (int x, int y) _dimensions;

    public Robot((int x, int y) position, (int x, int y) velocity, (int x, int y) dimensions)
    {
        _dimensions = dimensions;
        Position = position;
        Velocity = velocity;
    }

    public void Move()
    {
        var newPosition = Position.Add(Velocity);

        if (newPosition.x > _dimensions.x - 1)
            newPosition.x -= _dimensions.x;
        else if (newPosition.x < 0)
            newPosition.x = _dimensions.x + newPosition.x;
        if (newPosition.y > _dimensions.y - 1)
            newPosition.y -= _dimensions.y;
        else if (newPosition.y < 0)
            newPosition.y = _dimensions.y + newPosition.y;

        Position = newPosition;
    }

    public (int x, int y) Position { get; private set; }
    public (int x, int y) Velocity { get; init; }
}

public static class TupleExtensions14
{
    public static (int x, int y) Add(this (int x, int y) tuple1, (int x, int y) tuple2)
    {
        return (tuple1.x + tuple2.x, tuple1.y + tuple2.y);
    }
}
