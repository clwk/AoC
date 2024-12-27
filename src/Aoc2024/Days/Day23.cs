using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day23
{
    private readonly ILogger _logger;
    private readonly string[] _input;
    private readonly Dictionary<string, List<string>> _connections = [];
    private Dictionary<string, HashSet<string>> _adjacencyMatrix;

    public Day23(string[] input, ILogger logger)
    {
        _input = input;
        _logger = logger;
        ParseInput();
        CreateAdjacencyMatrix();
    }

    private void ParseInput()
    {
        foreach (var line in _input)
        {
            var parts = line.Split('-');
            if (parts.Length != 2)
                continue;

            var computer1 = parts[0];
            var computer2 = parts[1];

            if (!_connections.TryGetValue(computer1, out List<string>? list1))
            {
                list1 = [];
                _connections[computer1] = list1;
            }
            if (!_connections.TryGetValue(computer2, out List<string>? list2))
            {
                list2 = [];
                _connections[computer2] = list2;
            }

            list1.Add(computer2);
            list2.Add(computer1);
        }
    }

    private void CreateAdjacencyMatrix()
    {
        _adjacencyMatrix = new Dictionary<string, HashSet<string>>();

        foreach (var connection in _connections)
        {
            if (!_adjacencyMatrix.ContainsKey(connection.Key))
            {
                _adjacencyMatrix[connection.Key] = [];
            }

            foreach (var neighbor in connection.Value)
            {
                _adjacencyMatrix[connection.Key].Add(neighbor);

                if (!_adjacencyMatrix.ContainsKey(neighbor))
                {
                    _adjacencyMatrix[neighbor] = [];
                }

                _adjacencyMatrix[neighbor].Add(connection.Key);
            }
        }
    }

    private List<string> FindLargestClique()
    {
        var allNodes = new HashSet<string>(_adjacencyMatrix.Keys);
        return RunBronKerbosch([], allNodes, []);
    }

    private List<string> RunBronKerbosch(
        HashSet<string> currentClique,
        HashSet<string> potentialNodes,
        HashSet<string> excludedNodes
    )
    {
        if (potentialNodes.Count == 0 && excludedNodes.Count == 0)
        {
            return [.. currentClique];
        }

        var largestClique = new List<string>();
        var pivot = potentialNodes.Union(excludedNodes).First();
        var neighbors = _adjacencyMatrix[pivot];

        foreach (var node in potentialNodes.Except(neighbors).ToList())
        {
            var newClique = new HashSet<string>(currentClique) { node };
            var newPotentialNodes = new HashSet<string>(
                potentialNodes.Intersect(_adjacencyMatrix[node])
            );
            var newExcludedNodes = new HashSet<string>(
                excludedNodes.Intersect(_adjacencyMatrix[node])
            );
            var candidateClique = RunBronKerbosch(newClique, newPotentialNodes, newExcludedNodes);

            if (candidateClique.Count > largestClique.Count)
            {
                largestClique = candidateClique;
            }

            potentialNodes.Remove(node);
            excludedNodes.Add(node);
        }

        return largestClique;
    }

    public int RunPart1()
    {
        var threeConnections = new HashSet<(string, string, string)>();

        foreach (var connection1 in _connections)
        {
            var computer1 = connection1.Key;
            var neighbors1 = new HashSet<string>(connection1.Value);

            foreach (var computer2 in neighbors1)
            {
                if (!_connections.TryGetValue(computer2, out var connection))
                    continue;

                var neighbors2 = new HashSet<string>(connection);

                foreach (var computer3 in neighbors1.Intersect(neighbors2))
                {
                    var sortedConnection = new List<string> { computer1, computer2, computer3 };
                    sortedConnection.Sort();
                    threeConnections.Add(
                        (sortedConnection[0], sortedConnection[1], sortedConnection[2])
                    );
                }
            }
        }

        var connectionsWithT = threeConnections
            .Where(c =>
                c.Item1.StartsWith('t') || c.Item2.StartsWith('t') || c.Item3.StartsWith('t')
            )
            .ToList();

        _logger.LogInformation("Connected with 't': {@connectedWithT}", connectionsWithT);

        return connectionsWithT.Count;
    }

    public string RunPart2()
    {
        CreateAdjacencyMatrix();
        var largestClique = FindLargestClique().Order().ToList();
        _logger.LogInformation("Largest clique: {@largestClique}", largestClique);

        return string.Join(',', largestClique);
    }
}
