namespace Aoc2024.Days;

public class Day05
{
    private readonly string[] _input;
    private readonly Dictionary<int, HashSet<int>> _beforeListByAfter;
    private readonly List<List<int>> _updatePageNumbers;

    public Day05(string[] input)
    {
        _input = input;

        // Find the index of the empty line that splits the input
        var splitIndex = Array.FindIndex(_input, string.IsNullOrEmpty);

        _beforeListByAfter = ParseBeforeListByAfter(splitIndex);
        _updatePageNumbers = ParseUpdatePageNumbers(splitIndex);
    }

    private List<List<int>> ParseUpdatePageNumbers(int splitIndex) =>
        _input
            .Skip(splitIndex + 1)
            .Select(line => line.Split(',').Select(int.Parse).ToList())
            .ToList();

    private Dictionary<int, HashSet<int>> ParseBeforeListByAfter(int splitIndex) =>
        _input
            .Take(splitIndex)
            .Select(line => line.Split('|'))
            .GroupBy(parts => int.Parse(parts[1]))
            .ToDictionary(
                group => group.Key,
                group => group.Select(parts => int.Parse(parts[0])).ToHashSet()
            );

    public int RunPart1()
    {
        var validLines = _updatePageNumbers.Where(IsUpdateValid).ToList();
        var sumOfMiddles = GetSumOfMiddle(validLines);
        return sumOfMiddles;
    }

    private static int GetSumOfMiddle(List<List<int>> lines)
    {
        return lines.Sum(line => line[(line.Count - 1) / 2]);
    }

    private bool IsUpdateValid(List<int> line)
    {
        for (var i = 0; i < line.Count; i++)
        {
            if (_beforeListByAfter.TryGetValue(line[i], out var checklist))
            {
                if (checklist.Intersect(line.Skip(i)).Any())
                {
                    return false;
                }
            }
        }

        return true;
    }

    public int RunPart2()
    {
        var reorderedLines = GetReorderedLines();
        var sum = GetSumOfMiddle(reorderedLines);
        return sum;
    }

    private List<List<int>> GetReorderedLines()
    {
        var invalidLines = _updatePageNumbers.Where(line => !IsUpdateValid(line)).ToList();
        var reorderedLines = new List<List<int>>();

        foreach (var line in invalidLines)
        {
            var newLine = new List<int>();
            while (newLine.Count < line.Count)
            {
                var foundDigit = FindNextDigit(line, newLine);
                if (foundDigit != null)
                {
                    newLine.Add(foundDigit.Value);
                }
            }

            reorderedLines.Add(newLine);
        }

        return reorderedLines;
    }

    private int? FindNextDigit(List<int> line, List<int> newLine)
    {
        foreach (var digit in line.Except(newLine))
        {
            if (_beforeListByAfter.TryGetValue(digit, out var checklist))
            {
                if (!line.Except(newLine).Except([digit]).Intersect(checklist).Any())
                {
                    return digit;
                }
            }
            else
            {
                return digit;
            }
        }

        return null;
    }
}
