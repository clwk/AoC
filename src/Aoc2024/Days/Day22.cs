using Microsoft.Extensions.Logging;

namespace Aoc2024.Days;

public class Day22
{
    private const long PruneNumber = 16777216;

    private readonly ILogger _logger;

    private readonly List<long> _inputNumbers;
    private readonly List<List<int>> _prices = [];
    private readonly List<List<int>> _pricesDiffs = [];

    public Day22(string[] input, ILogger logger)
    {
        _logger = logger;
        _inputNumbers = input.Select(long.Parse).ToList();
    }

    public long RunPart1()
    {
        long sum = 0;

        foreach (var nr in _inputNumbers)
        {
            var current = nr;
            for (var i = 0; i < 2000; i++)
            {
                var nextNr = GetNextSecretNumber(current);
                current = nextNr;
            }

            _logger.LogDebug("{initial} 2000th {2000th}", nr, current);

            sum += current;
        }

        _logger.LogInformation("Sum: {sum}", sum);
        return sum;
    }

    private static long GetNextSecretNumber(long current)
    {
        var first = ((current * 64) ^ current) % PruneNumber;

        var second = ((long)Math.Floor((double)(first / 32)) ^ first) % PruneNumber;

        var third = ((second * 2048) ^ second) % PruneNumber;
        return third;
    }

    public int RunPart2()
    {
        CalculatePriceDiffs();

        var maxSumSequence = FindSequenceWithMaximumSum();
        _logger.LogInformation(
            "Sequence with maximum sum {sum} in part 2: {@sequence}",
            maxSumSequence.maxSum,
            maxSumSequence.sequence
        );

        return maxSumSequence.maxSum;
    }

    private Dictionary<List<int>, Dictionary<int, int>> BuildSequenceIndex()
    {
        var dict = new Dictionary<List<int>, Dictionary<int, int>>(new SequenceComparer());
        for (var i = 0; i < _pricesDiffs.Count; i++)
        {
            var diffs = _pricesDiffs[i];
            for (var start = 0; start <= diffs.Count - 4; start++)
            {
                var seq = diffs.Skip(start).Take(4).ToList();
                if (!dict.ContainsKey(seq))
                    dict[seq] = new Dictionary<int, int>();

                // only add the first occurrence for each list index
                if (!dict[seq].ContainsKey(i))
                    dict[seq][i] = start;
            }
        }
        return dict;
    }

    private (List<int> sequence, int maxSum) FindSequenceWithMaximumSum()
    {
        var sequenceIndex = BuildSequenceIndex();
        List<int> maxSequence = [];
        var maxSum = int.MinValue;

        foreach (var kvp in sequenceIndex)
        {
            var sum = 0;
            foreach (var (listIndex, pos) in kvp.Value)
            {
                if (pos + 4 < _prices[listIndex].Count)
                {
                    sum += _prices[listIndex][pos + 4];
                }
            }

            if (sum > maxSum)
            {
                maxSum = sum;
                maxSequence = kvp.Key;
            }
        }

        return (maxSequence, maxSum);
    }

    private void CalculatePriceDiffs()
    {
        foreach (var nr in _inputNumbers)
        {
            var current = nr;
            List<int> prices = [(int)(current % 10)];

            for (var i = 0; i < 2000; i++)
            {
                var nextNr = GetNextSecretNumber(current);
                current = nextNr;
                prices.Add((int)(current % 10));
            }

            _prices.Add(prices);
            var differences = prices.Zip(prices.Skip(1), (prev, curr) => curr - prev).ToList();
            _pricesDiffs.Add(differences);

            _logger.LogDebug("{initial} 2000th {2000th}", nr, current);
        }
    }
}

internal class SequenceComparer : IEqualityComparer<List<int>>
{
    public bool Equals(List<int>? x, List<int>? y)
    {
        return x != null && y != null && x.SequenceEqual(y);
    }

    public int GetHashCode(List<int> obj)
    {
        unchecked
        {
            int hash = 19;
            foreach (var item in obj)
            {
                hash = hash * 31 + item.GetHashCode();
            }
            return hash;
        }
    }
}
