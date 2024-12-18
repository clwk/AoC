using System.Text;
using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day18
{
    private readonly ILogger _logger;
    private readonly string[] _allInput;
    private readonly string[] _firstInput;
    private int _nextInputLine = 0;

    private readonly Dictionary<Direction4Way, (int, int)> _directions = new()
    {
        { Direction4Way.Up, (0, -1) },
        { Direction4Way.Down, (0, 1) },
        { Direction4Way.Left, (-1, 0) },
        { Direction4Way.Right, (1, 0) },
    };

    private readonly (int x, int y) _dimensions;
    private readonly Dictionary<(int, int), List<(int, int)>> _graph = [];
    private readonly HashSet<(int, int)> _obstacles = [];

    public Day18(string[] input, ILogger logger, int mazeDimension, int nrOfBytes)
    {
        _dimensions = (mazeDimension, mazeDimension);
        _allInput = input;
        _firstInput = input.Take(nrOfBytes).ToArray();
        _nextInputLine = nrOfBytes;
        _logger = logger;

        ParseInputAndBuildGraph();
    }

    public int RunPart1()
    {
        PrintMap();
        var shortestPath = FindShortestPath((0, 0), _dimensions);
        return shortestPath.Count - 1;
    }

    public string RunPart2()
    {
        (int x, int y) obstacleCoordinate = (0, 0);

        while (FindShortestPath((0, 0), _dimensions).Count > 0)
        {
            var newObstacle = _allInput.Skip(_nextInputLine).First();
            obstacleCoordinate = ParseCoordinate(newObstacle);
            AddObstacle(obstacleCoordinate);
            _nextInputLine++;

            _logger.LogDebug("Added obstacle at: {@obstacle}", obstacleCoordinate);
        }

        _logger.LogInformation("Last obstacle: {@obstacle}", obstacleCoordinate);

        return $"({obstacleCoordinate.x},{obstacleCoordinate.y})";
    }

    private void ParseInputAndBuildGraph()
    {
        foreach (var line in _firstInput)
        {
            var obstacleCoordinate = ParseCoordinate(line);
            _obstacles.Add(obstacleCoordinate);
        }

        for (var x = 0; x <= _dimensions.x; x++)
        {
            for (var y = 0; y <= _dimensions.y; y++)
            {
                var node = (x, y);
                if (_obstacles.Contains(node))
                    continue;

                _graph[node] = [];

                foreach (var direction in _directions.Values)
                {
                    var neighbor = (x + direction.Item1, y + direction.Item2);
                    if (IsValidCoordinate(neighbor))
                    {
                        _graph[node].Add(neighbor);
                    }
                }
            }
        }
    }

    private static (int x, int y) ParseCoordinate(string line)
    {
        var parts = line.Split(',');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    private bool IsValidCoordinate((int, int) coordinate)
    {
        var (x, y) = coordinate;
        var withinBounds = x >= 0 && x <= _dimensions.x && y >= 0 && y <= _dimensions.y;
        var isNotObstacle = !_obstacles.Contains(coordinate);
        return withinBounds && isNotObstacle;
    }

    private void PrintMap()
    {
        var sb = new StringBuilder();
        for (var y = 0; y <= _dimensions.y; y++)
        {
            for (var x = 0; x <= _dimensions.x; x++)
            {
                sb.Append(_obstacles.Contains((x, y)) ? '#' : '.');
            }
            sb.AppendLine();
        }

        _logger.LogInformation("{lines}", sb.ToString());
    }

    private List<(int, int)> FindShortestPath((int, int) start, (int, int) end)
    {
        var queue = new Queue<(int, int)>();
        var cameFrom = new Dictionary<(int, int), (int, int)?>();
        var visited = new HashSet<(int, int)>();

        queue.Enqueue(start);
        cameFrom[start] = null;
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == end)
            {
                var path = new List<(int, int)>();
                (int, int)? currentPathNode = current;
                while (currentPathNode != null)
                {
                    path.Add(currentPathNode.Value);
                    currentPathNode = cameFrom[currentPathNode.Value];
                }
                path.Reverse();
                return path;
            }

            foreach (var neighbor in _graph[current])
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return []; // No path found
    }

    private void AddObstacle((int, int) obstacle)
    {
        if (!_obstacles.Add(obstacle))
            return;

        // Remove the obstacle from the graph
        _graph.Remove(obstacle);

        // Update neighbors
        foreach (var direction in _directions.Values)
        {
            var neighbor = (obstacle.Item1 + direction.Item1, obstacle.Item2 + direction.Item2);
            if (_graph.TryGetValue(neighbor, out var neighbors))
            {
                neighbors.Remove(obstacle);
            }
        }
    }
}
